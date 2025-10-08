using MemoriesVietnam.Domain.Enum;
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
            var order = await _orderRepo.GetByIdAsync(orderId.ToString());
            if (order == null) return null;

            string vnp_Url = _config["Vnpay:Url"];
            string vnp_Returnurl = _config["Vnpay:ReturnUrl"];
            string vnp_TmnCode = _config["Vnpay:TmnCode"];
            string vnp_HashSecret = _config["Vnpay:HashSecret"];

            var vnp_Params = new Dictionary<string, string>
            {
                {"vnp_Version", "2.1.0"},
                {"vnp_Command", "pay"},
                {"vnp_TmnCode", vnp_TmnCode},
                {"vnp_Amount", ((int)order.Total * 100).ToString()},
                {"vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")},
                {"vnp_CurrCode", "VND"},
                {"vnp_IpAddr", context.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1"},
                {"vnp_Locale", "vn"},
                {"vnp_OrderInfo", $"Thanh toan don hang {order.Id}"},
                {"vnp_OrderType", "other"},
                {"vnp_ReturnUrl", vnp_Returnurl},
                {"vnp_TxnRef", order.Id.ToString()}
            };

            // 🔹 Sắp xếp theo key, build query string
            var sorted = vnp_Params.OrderBy(x => x.Key);
            var query = new StringBuilder();
            var signData = new StringBuilder();

            foreach (var kv in sorted)
            {
                query.Append(Uri.EscapeDataString(kv.Key) + "=" + Uri.EscapeDataString(kv.Value) + "&");
                signData.Append(kv.Key + "=" + kv.Value + "&");
            }

            string rawData = signData.ToString().TrimEnd('&');
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, rawData);
            query.Append("vnp_SecureHash=" + vnp_SecureHash);

            string paymentUrl = vnp_Url + "?" + query.ToString();

            return paymentUrl;
        }

        // 🔹 Xử lý callback từ VNPAY
        public async Task<object> HandleVnpayReturn(IQueryCollection query)
        {
            string vnp_HashSecret = _config["Vnpay:HashSecret"];
            var vnpData = query
                .Where(x => x.Key.StartsWith("vnp_"))
                .ToDictionary(x => x.Key, x => x.Value.ToString());

            string vnp_SecureHash = vnpData["vnp_SecureHash"];
            vnpData.Remove("vnp_SecureHash");
            vnpData.Remove("vnp_SecureHashType");

            string rawData = string.Join("&", vnpData.OrderBy(x => x.Key).Select(kv => kv.Key + "=" + kv.Value));
            string checkHash = HmacSHA512(vnp_HashSecret, rawData);

            if (checkHash == vnp_SecureHash)
            {
                string orderId = vnpData["vnp_TxnRef"];
                string responseCode = vnpData["vnp_ResponseCode"]; // ✅ Kiểm tra trạng thái thanh toán

                if (responseCode == "00") // 00 = Thành công
                {
                    await _orderRepo.UpdateOrderStatus(orderId, "Paid");

                    return new
                    {
                        Success = true,
                        Message = "Thanh toán thành công",
                        OrderId = orderId
                    };
                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "Thanh toán không thành công (ResponseCode = " + responseCode + ")"
                    };
                }
            }

            return new { Success = false, Message = "Xác thực thất bại" };
        }

        private static string HmacSHA512(string key, string inputData)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using var hmac = new HMACSHA512(keyBytes);
            var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        }
    }
}
