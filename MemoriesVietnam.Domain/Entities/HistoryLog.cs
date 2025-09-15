using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Enum;

namespace MemoriesVietnam.Domain.Entities
{
    public class HistoryLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public TargetType TargetType { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string TargetId { get; set; }
        public float Progress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
