using System.ComponentModel.DataAnnotations;

namespace MemoriesVietnam.Models.Entities
{
    public class Podcast
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? CoverUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<PodcastEpisode> Episodes { get; set; } = new List<PodcastEpisode>();
    }
}
