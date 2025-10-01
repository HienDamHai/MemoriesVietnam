using MemoriesVietnam.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string TargetId { get; set; }
        public TargetType TargetType { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ParentId { get; set; }
    }

    public class CreateCommentDto
    {
        public string TargetId { get; set; }
        public TargetType TargetType { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? ParentId { get; set; }
    }

    public class UpdateCommentDto
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
    }
}
