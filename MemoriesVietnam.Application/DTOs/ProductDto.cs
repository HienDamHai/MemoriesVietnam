using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Slug { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Images { get; set; }
        public string CategoryId { get; set; } = "";
    }

    public class CreateProductDto
    {
        public string Name { get; set; } = "";
        public string Slug { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; } = 0;
        public string? Images { get; set; }
        public string CategoryId { get; set; } = "";
    }

    public class UpdateProductDto : CreateProductDto
    {
        public string Id { get; set; } = "";
    }
}
