using MemoriesVietnam.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class LikeTableDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string TargetId { get; set; }
        public TargetType TargetType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateLikeTableDto
    {
        public string TargetId { get; set; }
        public TargetType TargetType { get; set; }
    }
}
