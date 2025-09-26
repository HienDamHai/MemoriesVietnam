using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Order>().GetAllAsync();
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<Order>().GetByIdAsync(id);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            order.Total = order.OrderItems?.Sum(i => i.Price * i.Qty) ?? 0;
            order.Status = OrderStatus.Pending;

            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order;
        }

        public async Task<Order?> UpdateAsync(string id, OrderStatus status)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order == null) return null;

            order.Status = status;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.SaveChangesAsync();

            return order;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order == null) return false;
            _unitOfWork.Repository<Order>().Remove(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
