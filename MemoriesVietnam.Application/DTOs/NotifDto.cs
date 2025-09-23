using MemoriesVietnam.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class NotifDto
    {
        public class NotificationCreateRequest
        {
            public string UserId { get; set; }
            public string Type { get; set; }
            public string Content { get; set; }
            public string? TargetId { get; set; }
            public TargetType? TargetType { get; set; }
        }

        public class NotificationResponse
        {
            public string Id { get; set; }
            public string UserId { get; set; }
            public string Type { get; set; }
            public string Content { get; set; }
            public string? TargetId { get; set; }
            public TargetType? TargetType { get; set; }
            public bool IsRead { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
