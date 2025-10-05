using MemoriesVietnam.Models.Entities;

namespace MemoriesVietnam.Models.DTOs
{
    public class CommentDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
        public CommentTargetType TargetType { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ParentId { get; set; }
        public UserDto? User { get; set; }
        public List<CommentDto> Replies { get; set; } = new();
    }

    public class CreateCommentDto
    {
        public string TargetId { get; set; } = string.Empty;
        public CommentTargetType TargetType { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ParentId { get; set; }
    }

    public class UpdateCommentDto
    {
        public string Content { get; set; } = string.Empty;
    }

    public class LikeToggleDto
    {
        public string TargetId { get; set; } = string.Empty;
        public TargetType TargetType { get; set; }
    }

    public class LikeResponseDto
    {
        public bool Liked { get; set; }
    }

    public class LikeStatusDto
    {
        public int Count { get; set; }
        public bool IsLiked { get; set; }
    }

    public class BookmarkToggleDto
    {
        public string TargetId { get; set; } = string.Empty;
        public BookmarkTargetType TargetType { get; set; }
    }

    public class BookmarkResponseDto
    {
        public bool Bookmarked { get; set; }
    }

    public class BookmarkStatusDto
    {
        public bool IsBookmarked { get; set; }
    }

    public class BookmarkDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
        public BookmarkTargetType TargetType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
