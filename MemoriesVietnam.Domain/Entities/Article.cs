using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Common;

namespace MemoriesVietnam.Domain.Entities
{
    public class Article : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = "";
        public string Slug { get; set; } = "";
        public string Content { get; set; } = "";
        public string CoverUrl { get; set; } = "";
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string EraId { get; set; }
        public Era Era { get; set; }
        public string? Sources { get; set; } // JSON
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public ICollection<ArticleAudio>? ArticleAudios { get; set; }
        public ICollection<ArticleTag>? ArticleTags { get; set; }
    }
}
