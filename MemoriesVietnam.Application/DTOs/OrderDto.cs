using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class OrderItemDto
    {
        public string ProductId { get; set; } = "";
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderDto
    {
        public string Id { get; set; } = "";
        public string? UserId { get; set; }
        public decimal Total { get; set; }
        public string Payment { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "";
        public List<OrderItemDto> Items { get; set; } = new();
    }


    public class CreateOrderDto
    {
        public string? UserId { get; set; }
        public string Payment { get; set; } = "";
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class UpdateOrderStatusDto
    {
        public string OrderId { get; set; } = "";
        public string Status { get; set; } = "";
    }


    public class CreateOrderItemDto
    {
        public string ProductId { get; set; } = "";
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}
