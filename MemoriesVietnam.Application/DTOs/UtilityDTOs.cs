using MemoriesVietnam.Models.Entities;

namespace MemoriesVietnam.Models.DTOs
{
    public class HistoryLogDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
        public HistoryTargetType TargetType { get; set; }
        public float Progress { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateHistoryLogDto
    {
        public string TargetId { get; set; } = string.Empty;
        public HistoryTargetType TargetType { get; set; }
        public float Progress { get; set; }
    }

    public class NotificationDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? TargetId { get; set; }
        public NotificationTargetType? TargetType { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TagDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTagDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UploadResponseDto
    {
        public string Url { get; set; } = string.Empty;
    }
}
