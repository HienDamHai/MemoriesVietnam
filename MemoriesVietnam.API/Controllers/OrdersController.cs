using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Services.Interfaces;
using System.Security.Claims;

namespace MemoriesVietnam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderDto>>> Create([FromBody] CreateOrderDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<OrderDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var order = await _orderService.CreateAsync(userId, dto);
                return Ok(new ApiResponse<OrderDto>
                {
                    Data = order,
                    Message = "Tạo đơn hàng thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<OrderDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("my")]
        public async Task<ActionResult<ApiResponse<List<OrderDto>>>> GetMyOrders()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<List<OrderDto>>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var orders = await _orderService.GetMyOrdersAsync(userId);
                return Ok(new ApiResponse<List<OrderDto>>
                {
                    Data = orders,
                    Message = "Lấy danh sách đơn hàng thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<OrderDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetById(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                
                // Admin có thể xem tất cả đơn hàng, user chỉ xem đơn hàng của mình
                var order = await _orderService.GetByIdAsync(id, userRole == "Admin" ? null : userId);
                
                if (order == null)
                {
                    return NotFound(new ApiResponse<OrderDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy đơn hàng",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<OrderDto>
                {
                    Data = order,
                    Message = "Lấy thông tin đơn hàng thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<OrderDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateStatus(string id, [FromBody] UpdateOrderStatusDto dto)
        {
            try
            {
                var order = await _orderService.UpdateStatusAsync(id, dto.Status);
                if (order == null)
                {
                    return NotFound(new ApiResponse<OrderDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy đơn hàng",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<OrderDto>
                {
                    Data = order,
                    Message = "Cập nhật trạng thái đơn hàng thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<OrderDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<OrderDto>>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(new ApiResponse<List<OrderDto>>
                {
                    Data = orders,
                    Message = "Lấy danh sách tất cả đơn hàng thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<OrderDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
