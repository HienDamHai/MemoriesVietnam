using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Application.Services;
using System.Threading.Tasks;

namespace MemoriesVietnam.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // 🔹 Tạo link thanh toán
        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentUrl([FromQuery] string orderId)
        {
            var url = await _paymentService.CreatePaymentUrl(orderId, HttpContext);
            if (url == null) return NotFound("Không tìm thấy đơn hàng");
            return Ok(new { paymentUrl = url });
        }


        // 🔹 Callback từ VNPAY (sau khi thanh toán)
        [HttpGet("vnpay-return")]
        public async Task<IActionResult> PaymentReturn()
        {
            var response = await _paymentService.HandleVnpayReturn(Request.Query);
            return Ok(response);
        }
    }
}
