using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Enum;

namespace MemoriesVietnam.Domain.Entities
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public User? User { get; set; }
        public decimal Total { get; set; }
        public string Payment { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
