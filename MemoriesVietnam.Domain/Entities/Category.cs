using System.ComponentModel.DataAnnotations;

namespace MemoriesVietnam.Models.Entities
{
    public class Category
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
