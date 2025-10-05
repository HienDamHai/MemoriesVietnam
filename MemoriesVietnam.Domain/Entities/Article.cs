using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MemoriesVietnam.Models.Entities
{
    public class Article
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? CoverUrl { get; set; }

        public int YearStart { get; set; }

        public int YearEnd { get; set; }

        [Required]
        [StringLength(100)]
        public string EraId { get; set; } = string.Empty;

        public string? Sources { get; set; } // JSON string

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("EraId")]
        public virtual Era Era { get; set; } = null!;

        public virtual ICollection<ArticleAudio> ArticleAudios { get; set; } = new List<ArticleAudio>();
        public virtual ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
        public virtual ICollection<PodcastEpisode> PodcastEpisodes { get; set; } = new List<PodcastEpisode>();

        // Helper property to work with Sources as object
        [NotMapped]
        public object? SourcesObject
        {
            get => string.IsNullOrEmpty(Sources) ? null : JsonSerializer.Deserialize<object>(Sources);
            set => Sources = value == null ? null : JsonSerializer.Serialize(value);
        }
    }
}
