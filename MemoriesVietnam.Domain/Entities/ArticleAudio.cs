using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Common;

namespace MemoriesVietnam.Domain.Entities
{
    public class ArticleAudio : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ArticleId { get; set; }
        public Article Article { get; set; }
        public string VoiceId { get; set; } = "";
        public string Url { get; set; } = "";
        public int Duration { get; set; }

        public string CreatedBy { get; set; }
        public User CreatedByUser { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
