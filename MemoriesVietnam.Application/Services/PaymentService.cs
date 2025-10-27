using MemoriesVietnam.Domain.IRepositories;
using Microsoft.AspNetCore.Http;
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
        private readonly IOrderRepository _orderRepo;

        // 🔹 Cấu hình trực tiếp, dùng IPN server cho vnp_ReturnUrl
        private const string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private const string vnp_ReturnUrl = "https://memoirsvietnam-faa3hydzbwhbdnhe.southeastasia-01.azurewebsites.net/api/Payment/vnpay-return";
        private const string vnp_TmnCode = "8D3KC89F";
        private const string vnp_HashSecret = "U4ENMXYM8TM3M47QW7P3DI0UKKH9QRJ0";

        public PaymentService(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        // 🔹 Tạo link thanh toán VNPAY
        public async Task<string?> CreatePaymentUrl(string orderId, HttpContext context)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
                return null;

            var vnp_Params = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", ((long)Math.Round(order.Total * 100)).ToString() }, // VND * 100
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", GetClientIpAddress(context) },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", $"Thanh toan don hang {order.Id}" },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", vnp_ReturnUrl },
                { "vnp_TxnRef", order.Id }
            };

            // 🔹 Tạo chữ ký
            string rawData = string.Join("&", vnp_Params.OrderBy(kv => kv.Key)
                .Select(kv => $"{kv.Key}={kv.Value}"));
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, rawData);

            // 🔹 Tạo query string encode
            string queryString = string.Join("&", vnp_Params.OrderBy(kv => kv.Key)
                .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

            return $"{vnp_Url}?{queryString}&vnp_SecureHashType=SHA512&vnp_SecureHash={vnp_SecureHash}";
        }

        // 🔹 Xử lý IPN / callback từ VNPAY
        public async Task<object> HandleVnpayReturn(IQueryCollection query)
        {
            var vnpData = query
                .Where(x => x.Key.StartsWith("vnp_"))
                .ToDictionary(x => x.Key, x => x.Value.ToString());

            if (!vnpData.TryGetValue("vnp_SecureHash", out string vnp_SecureHash))
                return new { Success = false, Message = "Thiếu chữ ký xác thực." };

            // 🔹 Xoá key không dùng để kiểm tra hash
            vnpData.Remove("vnp_SecureHash");
            vnpData.Remove("vnp_SecureHashType");

            string rawData = string.Join("&", vnpData.OrderBy(x => x.Key)
                .Select(kv => $"{kv.Key}={kv.Value}"));
            string checkHash = HmacSHA512(vnp_HashSecret, rawData);

            if (!checkHash.Equals(vnp_SecureHash, StringComparison.OrdinalIgnoreCase))
                return new { Success = false, Message = "Sai chữ ký xác thực (Hash mismatch)." };

            string orderId = vnpData["vnp_TxnRef"];
            string responseCode = vnpData.GetValueOrDefault("vnp_ResponseCode", "99");

            if (responseCode == "00")
            {
                await _orderRepo.UpdateOrderStatus(orderId, "Paid");
                return new { Success = true, Message = "Thanh toán thành công", OrderId = orderId };
            }

            return new { Success = false, Message = $"Thanh toán thất bại (ResponseCode = {responseCode})" };
        }

        private static string HmacSHA512(string key, string data)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private string GetClientIpAddress(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            return string.IsNullOrEmpty(ip) || ip == "::1" ? "127.0.0.1" : ip;
        }
    }
}
