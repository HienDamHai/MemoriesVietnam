using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class ArticleAudioDto
    {
        public string Id { get; set; }
        public string ArticleId { get; set; }
        public string VoiceId { get; set; }
        public string Url { get; set; }
        public int Duration { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateArticleAudioDto
    {
        public string ArticleId { get; set; }
        public string VoiceId { get; set; }
        public string Url { get; set; }
        public int Duration { get; set; }
    }

    public class UpdateArticleAudioDto
    {
        public string VoiceId { get; set; }
        public string Url { get; set; }
        public int Duration { get; set; }
    }
}
