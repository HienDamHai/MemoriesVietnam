using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MemoriesVietnam.Models.Entities
{
    public class Product
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; } = 0;

        public string? Images { get; set; } // JSON string array

        [Required]
        [StringLength(100)]
        public string CategoryId { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Helper property to work with Images as string array
        [NotMapped]
        public string[]? ImageArray
        {
            get => string.IsNullOrEmpty(Images) ? null : JsonSerializer.Deserialize<string[]>(Images);
            set => Images = value == null ? null : JsonSerializer.Serialize(value);
        }
    }
}
