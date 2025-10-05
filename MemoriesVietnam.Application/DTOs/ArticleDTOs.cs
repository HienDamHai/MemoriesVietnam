using MemoriesVietnam.Models.Entities;

namespace MemoriesVietnam.Models.DTOs
{
    public class ArticleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? CoverUrl { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string EraId { get; set; } = string.Empty;
        public object? Sources { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public EraDto? Era { get; set; }
        public List<ArticleAudioDto> ArticleAudios { get; set; } = new();
        public List<TagDto> Tags { get; set; } = new();
    }

    public class ArticleAudioDto
    {
        public string Id { get; set; } = string.Empty;
        public string ArticleId { get; set; } = string.Empty;
        public string VoiceId { get; set; } = string.Empty;
        public string? Url { get; set; }
        public int Duration { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class CreateArticleDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? CoverUrl { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string EraId { get; set; } = string.Empty;
        public object? Sources { get; set; }
        public List<string> TagIds { get; set; } = new();
    }

    public class UpdateArticleDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? CoverUrl { get; set; }
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string? EraId { get; set; }
        public object? Sources { get; set; }
        public List<string>? TagIds { get; set; }
    }
}
