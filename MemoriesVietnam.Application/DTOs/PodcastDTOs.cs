namespace MemoriesVietnam.Models.DTOs
{
    public class PodcastDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PodcastEpisodeDto> Episodes { get; set; } = new();
    }

    public class PodcastEpisodeDto
    {
        public string Id { get; set; } = string.Empty;
        public string PodcastId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? AudioUrl { get; set; }
        public int Duration { get; set; }
        public string? ArticleId { get; set; }
        public int EpisodeNumber { get; set; }
        public ArticleDto? Article { get; set; }
    }

    public class CreatePodcastDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverUrl { get; set; }
    }

    public class CreatePodcastEpisodeDto
    {
        public string Title { get; set; } = string.Empty;
        public string? AudioUrl { get; set; }
        public int Duration { get; set; }
        public string? ArticleId { get; set; }
        public int EpisodeNumber { get; set; }
    }
}
