using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Models.Entities;

namespace MemoriesVietnam.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(string userId, CreateOrderDto dto);
        Task<List<OrderDto>> GetMyOrdersAsync(string userId);
        Task<OrderDto?> GetByIdAsync(string id, string? userId = null);
        Task<OrderDto?> UpdateStatusAsync(string id, OrderStatus status);
        Task<List<OrderDto>> GetAllOrdersAsync(); // Admin only
        Task<decimal> CalculateOrderTotal(List<CreateOrderItemDto> items);
    }
}
