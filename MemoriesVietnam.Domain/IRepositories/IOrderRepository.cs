using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetByIdAsync(string id);
        Task UpdateOrderStatus(string orderId, string status);
    }
}
