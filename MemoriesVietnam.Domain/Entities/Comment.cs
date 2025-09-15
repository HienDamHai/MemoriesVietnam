using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Common;
using MemoriesVietnam.Domain.Enum;

namespace MemoriesVietnam.Domain.Entities
{
    public class Comment : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; }
        public string TargetId { get; set; }
        public TargetType TargetType { get; set; }
        public string Content { get; set; } = "";
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ParentId { get; set; }
        public Comment? Parent { get; set; }
        public ICollection<Comment>? Replies { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
