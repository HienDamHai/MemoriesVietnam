using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesVietnam.Models.Entities
{
    public class HistoryLog
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string TargetId { get; set; } = string.Empty;

        public HistoryTargetType TargetType { get; set; }

        public float Progress { get; set; } // 0.0 to 1.0

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    public enum HistoryTargetType
    {
        Article,
        Audio,
        Podcast
    }
}
