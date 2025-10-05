using System.ComponentModel.DataAnnotations;

namespace MemoriesVietnam.Models.Entities
{
    public class Era
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public int YearStart { get; set; }

        public int YearEnd { get; set; }

        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
