using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesVietnam.Models.Entities
{
    public class ArticleAudio
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        public string ArticleId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string VoiceId { get; set; } = string.Empty;

        public string? Url { get; set; }

        public int Duration { get; set; } // in seconds

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; } = null!;
    }
}
