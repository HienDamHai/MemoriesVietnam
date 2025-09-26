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
    }
}
