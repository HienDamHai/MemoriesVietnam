using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.Entities
{
    public class OrderItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string OrderId { get; set; }
        public Order Order { get; set; }
        public string? ProductId { get; set; }
        public Product? Product { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}
