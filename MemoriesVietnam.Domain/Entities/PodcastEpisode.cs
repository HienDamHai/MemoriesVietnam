using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Common;

namespace MemoriesVietnam.Domain.Entities
{
    public class PodcastEpisode : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PodcastId { get; set; }
        public Podcast Podcast { get; set; }
        public string Title { get; set; } = "";
        public string AudioUrl { get; set; } = "";
        public int Duration { get; set; }
        public string? ArticleId { get; set; }
        public Article? Article { get; set; }
        public int EpisodeNumber { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
