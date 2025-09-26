using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class PodcastEpisodeDto
    {
        public class CreatePodcastEpisodeDto
        {
            public string PodcastId { get; set; } = "";
            public string Title { get; set; } = "";
            public string AudioUrl { get; set; } = "";
            public int Duration { get; set; }
            public int EpisodeNumber { get; set; }
            public string? ArticleId { get; set; }
        }

        public class UpdatePodcastEpisodeDto : CreatePodcastEpisodeDto
        {
            public string Id { get; set; } = "";
        }
    }
}
