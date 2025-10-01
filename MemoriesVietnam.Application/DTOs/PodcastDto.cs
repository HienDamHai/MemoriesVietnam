using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class PodcastDto
    {
        public class CreatePodcastDto
        {
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public string CoverUrl { get; set; } = "";
        }

        public class UpdatePodcastDto : CreatePodcastDto
        {
            public string Id { get; set; } = "";
        }
        public class PodcastHomeDto
        {
            public string Id { get; set; } = "";
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public string CoverUrl { get; set; } = "";
            public List<PodcastEpisodeDtoForHome> Episodes { get; set; } = new();

            public class PodcastEpisodeDtoForHome
            {
                public string Id { get; set; } = "";
                public string Title { get; set; } = "";
                public string AudioUrl { get; set; } = "";
                public string Duration { get; set; } = ""; // "mm:ss" cho frontend
                public int EpisodeNumber { get; set; }
            }
        }
    }
}

