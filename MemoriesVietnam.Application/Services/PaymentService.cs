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
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return null;

            // Đọc cấu hình
            string vnp_Url = _config["Vnpay:Url"];
            string vnp_Returnurl = _config["Vnpay:ReturnUrl"];
            string vnp_TmnCode = _config["Vnpay:TmnCode"];
            string vnp_HashSecret = _config["Vnpay:HashSecret"];

            // Tạo danh sách tham số gửi tới VNPAY
            var vnp_Params = new SortedList<string, string>(StringComparer.Ordinal)
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", ((long)(order.Total * 100)).ToString() }, // Nhân 100 theo quy định VNPAY
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", GetClientIpAddress(context) },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", $"Thanh toan don hang {order.Id}" },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", vnp_Returnurl },
                { "vnp_TxnRef", order.Id.ToString() }
            };

            // ✅ Tạo chuỗi rawData (không encode)
            var rawData = string.Join("&", vnp_Params.Select(x => $"{x.Key}={x.Value}"));

            // ✅ Tạo chữ ký
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, rawData);

            // ✅ Tạo URL (encode từng value)
            var queryString = string.Join("&", vnp_Params.Select(x =>
                $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value)}"));

            string paymentUrl = $"{vnp_Url}?{queryString}&vnp_SecureHash={vnp_SecureHash}";

            return paymentUrl;
        }

        // 🔹 Xử lý callback (Return URL)
        public async Task<object> HandleVnpayReturn(IQueryCollection query)
        {
            string vnp_HashSecret = _config["Vnpay:HashSecret"];

            // Lấy toàn bộ các tham số bắt đầu bằng "vnp_"
            var vnpData = query
                .Where(x => x.Key.StartsWith("vnp_"))
                .ToDictionary(x => x.Key, x => x.Value.ToString());

            // Lấy chữ ký từ response
            if (!vnpData.TryGetValue("vnp_SecureHash", out string vnp_SecureHash))
                return new { Success = false, Message = "Thiếu chữ ký xác thực." };

            vnpData.Remove("vnp_SecureHash");
            vnpData.Remove("vnp_SecureHashType");

            // Tạo lại rawData để xác thực chữ ký
            var orderedData = vnpData.OrderBy(x => x.Key, StringComparer.Ordinal);
            string rawData = string.Join("&", orderedData.Select(kv => $"{kv.Key}={kv.Value}"));
            string checkHash = HmacSHA512(vnp_HashSecret, rawData);

            // ✅ Kiểm tra chữ ký
            if (!checkHash.Equals(vnp_SecureHash, StringComparison.OrdinalIgnoreCase))
            {
                return new { Success = false, Message = "Sai chữ ký xác thực (Hash mismatch)." };
            }

            // ✅ Nếu chữ ký hợp lệ → kiểm tra mã phản hồi
            string orderId = vnpData["vnp_TxnRef"];
            string responseCode = vnpData["vnp_ResponseCode"];

            if (responseCode == "00")
            {
                await _orderRepo.UpdateOrderStatus(orderId, "Paid");
                return new
                {
                    Success = true,
                    Message = "Thanh toán thành công",
                    OrderId = orderId
                };
            }

            return new
            {
                Success = false,
                Message = $"Thanh toán thất bại (ResponseCode = {responseCode})"
            };
        }

        // 🔹 Hàm tạo HMAC SHA512
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
