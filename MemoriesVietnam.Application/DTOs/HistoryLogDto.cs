using MemoriesVietnam.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    
    namespace MemoriesVietnam.Application.DTOs
    {
        public class HistoryLogDto
        {
            public string Id { get; set; }
            public string? UserId { get; set; }
            public string TargetId { get; set; }
            public TargetType TargetType { get; set; }
            public float Progress { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class CreateHistoryLogDto
        {
            public string TargetId { get; set; }
            public TargetType TargetType { get; set; }
            public float Progress { get; set; }
        }
    }

}
