using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesVietnam.Models.Entities
{
    public class PodcastEpisode
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        public string PodcastId { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        public string? AudioUrl { get; set; }

        public int Duration { get; set; } // in seconds

        [StringLength(100)]
        public string? ArticleId { get; set; }

        public int EpisodeNumber { get; set; }

        // Navigation properties
        [ForeignKey("PodcastId")]
        public virtual Podcast Podcast { get; set; } = null!;

        [ForeignKey("ArticleId")]
        public virtual Article? Article { get; set; }
    }
}
