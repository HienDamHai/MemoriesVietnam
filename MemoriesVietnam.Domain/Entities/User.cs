using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesVietnam.Models.Entities
{
    public class User
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string LoginId { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("LoginId")]
        public virtual Login Login { get; set; } = null!;

        public virtual ICollection<ArticleAudio> ArticleAudios { get; set; } = new List<ArticleAudio>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<LikeTable> Likes { get; set; } = new List<LikeTable>();
        public virtual ICollection<HistoryLog> HistoryLogs { get; set; } = new List<HistoryLog>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
