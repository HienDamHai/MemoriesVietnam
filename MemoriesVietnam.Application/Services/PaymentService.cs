using MemoriesVietnam.Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class PaymentService
    {
        private readonly IConfiguration _config;
        private readonly IOrderRepository _orderRepo;

        public PaymentService(IConfiguration config, IOrderRepository orderRepo)
        {
            _config = config;
            _orderRepo = orderRepo;
        }

        // 🔹 Tạo link thanh toán VNPAY
        public async Task<string?> CreatePaymentUrl(string orderId, HttpContext context)
        {
            Console.WriteLine($"[PAYMENT] === BẮT ĐẦU TẠO LINK THANH TOÁN CHO ORDER: {orderId} ===");

            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                Console.WriteLine("[PAYMENT] ❌ Không tìm thấy đơn hàng!");
                return null;
            }

            // 🔹 Đọc cấu hình
            string vnp_Url = _config["Vnpay:Url"]!;
            string vnp_ReturnUrl = _config["Vnpay:ReturnUrl"]!;
            string vnp_TmnCode = _config["Vnpay:TmnCode"]!;
            string vnp_HashSecret = _config["Vnpay:HashSecret"]!;

            Console.WriteLine("[PAYMENT] 🔧 Cấu hình:");
            Console.WriteLine($"  vnp_Url = {vnp_Url}");
            Console.WriteLine($"  vnp_ReturnUrl = {vnp_ReturnUrl}");
            Console.WriteLine($"  vnp_TmnCode = {vnp_TmnCode}");
            Console.WriteLine($"  vnp_HashSecret (ẩn) = ****");

            // 🔹 Tạo danh sách tham số
            var vnp_Params = new SortedDictionary<string, string>(StringComparer.Ordinal)
{
    { "vnp_Version", "2.1.0" },
    { "vnp_Command", "pay" },
    { "vnp_TmnCode", vnp_TmnCode },
    { "vnp_Amount", ((long)(Math.Round(order.Total * 100))).ToString() },
    { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
    { "vnp_CurrCode", "VND" },
    { "vnp_IpAddr", GetClientIpAddress(context) },
    { "vnp_Locale", "vn" },
    { "vnp_OrderInfo", $"Thanh toan don hang {order.Id}" },
    { "vnp_OrderType", "other" },
    { "vnp_ReturnUrl", vnp_ReturnUrl },
    { "vnp_TxnRef", order.Id.ToString() }
};


            // 🔹 Tạo rawData
            string rawData = string.Join("&", vnp_Params
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .Select(kv => $"{kv.Key}={kv.Value}"));

            Console.WriteLine("[PAYMENT] 🔤 rawData:");
            Console.WriteLine(rawData);

            // 🔹 Tạo secure hash
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, rawData);
            Console.WriteLine($"[PAYMENT] 🔐 vnp_SecureHash = {vnp_SecureHash}");

            // 🔹 Tạo query string encode
            string queryString = string.Join("&", vnp_Params
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

            string paymentUrl = $"{vnp_Url}?{queryString}&vnp_SecureHashType=SHA512&vnp_SecureHash={vnp_SecureHash}";

            return paymentUrl;
        }

        // 🔹 Xử lý callback (Return URL)
        public async Task<object> HandleVnpayReturn(IQueryCollection query)
        {
            Console.WriteLine("[PAYMENT] === XỬ LÝ CALLBACK TỪ VNPAY ===");
            string vnp_HashSecret = _config["Vnpay:HashSecret"];

            var vnpData = query
                .Where(x => x.Key.StartsWith("vnp_"))
                .ToDictionary(x => x.Key, x => x.Value.ToString());

            if (!vnpData.TryGetValue("vnp_SecureHash", out string vnp_SecureHash))
            {
                Console.WriteLine("[PAYMENT] ❌ Thiếu vnp_SecureHash trong response!");
                return new { Success = false, Message = "Thiếu chữ ký xác thực." };
            }

            vnpData.Remove("vnp_SecureHash");
            vnpData.Remove("vnp_SecureHashType");

            string rawData = string.Join("&", vnpData.OrderBy(x => x.Key)
                .Select(kv => $"{kv.Key}={kv.Value}"));
            string checkHash = HmacSHA512(vnp_HashSecret, rawData);

            Console.WriteLine($"[PAYMENT] 🔎 rawData (verify): {rawData}");
            Console.WriteLine($"[PAYMENT] 🔎 checkHash: {checkHash}");
            Console.WriteLine($"[PAYMENT] 🔎 vnp_SecureHash (response): {vnp_SecureHash}");

            if (!checkHash.Equals(vnp_SecureHash, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("[PAYMENT] ❌ Sai chữ ký xác thực (Hash mismatch).");
                return new { Success = false, Message = "Sai chữ ký xác thực (Hash mismatch)." };
            }

            string orderId = vnpData["vnp_TxnRef"];
            string responseCode = vnpData["vnp_ResponseCode"];
            Console.WriteLine($"[PAYMENT] 📦 orderId = {orderId}, responseCode = {responseCode}");

            if (responseCode == "00")
            {
                await _orderRepo.UpdateOrderStatus(orderId, "Paid");
                Console.WriteLine("[PAYMENT] ✅ Thanh toán thành công.");
                return new { Success = true, Message = "Thanh toán thành công", OrderId = orderId };
            }

            Console.WriteLine($"[PAYMENT] ⚠️ Thanh toán thất bại. Mã lỗi = {responseCode}");
            return new { Success = false, Message = $"Thanh toán thất bại (ResponseCode = {responseCode})" };
        }

        private static string HmacSHA512(string key, string data)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using var hmac = new HMACSHA512(keyBytes);
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private string GetClientIpAddress(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ip) || ip == "::1")
                return "127.0.0.1"; // ✅ VNPAY chỉ chấp nhận IPv4
            return ip;
        }
    }
}
