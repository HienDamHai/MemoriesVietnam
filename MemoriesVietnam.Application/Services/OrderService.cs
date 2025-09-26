using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using MemoriesVietnam.Domain.IBasic;
using MemoriesVietnam.Domain.IRepositories;
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
        private readonly IOrderRepository _orderRepository;
        public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
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
            decimal total = 0;

            foreach (var item in order.OrderItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);

                if (product.Stock < item.Qty)
                {
                    throw new Exception($"Not enough stock for product {product.Name}");
                }

                item.Price = product.Price;
                total += item.Price * item.Qty;

                product.Stock -= item.Qty;
                _unitOfWork.Repository<Product>().Update(product);
            }

            order.Total = total;
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

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            if(string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            return orders;
        }
    }
}
