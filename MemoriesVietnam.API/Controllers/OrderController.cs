using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesVietnam.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();

            var dto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Total = order.Total,
                Payment = order.Payment,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Qty = i.Qty,
                    Price = i.Price
                }).ToList() ?? new List<OrderItemDto>()
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            var order = new Order
            {
                UserId = userId,
                Payment = dto.Payment,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Qty = i.Qty
                }).ToList()
            };

            var created = await _orderService.CreateAsync(order);

            var result = new OrderDto
            {
                Id = created.Id,
                UserId = created.UserId,
                Total = created.Total,
                Payment = created.Payment,
                Status = created.Status.ToString(),
                CreatedAt = created.CreatedAt,
                Items = created.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Qty = i.Qty,
                    Price = i.Price
                }).ToList()
            };

            return CreatedAtAction(nameof(GetMyOrderById), new { id = result.Id }, result);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateOrderStatusDto dto)
        {
            if (id != dto.OrderId) return BadRequest("ID mismatch");

            if(!Enum.TryParse<OrderStatus>(dto.Status, true, out var newStatus))
                return BadRequest("Invalid status value");

            var updated = await _orderService.UpdateAsync(id, newStatus);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _orderService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID cannot be null or empty");
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyOrder()
        {
            var userId = GetUserId();

            if (userId == null) return Unauthorized();

            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            var dtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = userId,
                Total = o.Total,
                Payment = o.Payment,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt,
                Items = o.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Qty = i.Qty,
                    Price = i.Price
                }).ToList()
            });

            return Ok(orders);
        }

        [HttpGet("me/{id}")]
        public async Task<IActionResult> GetMyOrderById(string id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var order = await _orderService.GetByIdAsync(id);
            if (order == null || order.UserId != userId) return NotFound();

            var dto = new OrderDto
            {
                Id = order.Id,
                UserId = userId,
                Total = order.Total,
                Payment = order.Payment,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Qty = i.Qty,
                    Price = i.Price
                }).ToList()
            };
            return Ok(dto);
        }


        
        private string GetUserId()
        {
            return User.FindFirst("userId")?.Value ?? "";
        }
    }
}
