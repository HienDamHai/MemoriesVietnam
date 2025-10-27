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

        // Tạo link thanh toán VNPAY
        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentUrl([FromQuery] string orderId)
        {
            var url = await _paymentService.CreatePaymentUrl(orderId, HttpContext);
            if (url == null) return NotFound(new { Message = "Không tìm thấy đơn hàng" });
            return Ok(new { paymentUrl = url });
        }

        // Callback VNPAY sau khi user thanh toán xong
        [HttpGet("vnpay-return")]
        public async Task<IActionResult> PaymentReturn()
        {
            var result = await _paymentService.HandleVnpayReturn(Request.Query);
            // Trả về JSON cho FE
            return Ok(result);
        }
    }
}
