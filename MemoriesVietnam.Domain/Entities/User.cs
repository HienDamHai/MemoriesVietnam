using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MemoriesVietnam.Domain.Common;

namespace MemoriesVietnam.Domain.Entities
{
    public class User : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime? VerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public string LoginId { get; set; }
        public Login Login { get; set; }

        // Navigation
        public ICollection<ArticleAudio>? ArticleAudios { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<LikeTable>? Likes { get; set; }
        public ICollection<HistoryLog>? HistoryLogs { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Bookmark>? Bookmarks { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
