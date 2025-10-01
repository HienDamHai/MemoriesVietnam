using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class ArticleDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string CoverUrl { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string EraId { get; set; }
        public string? Sources { get; set; }
        public DateTime? PublishedAt { get; set; }
    }

    public class CreateArticleDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string CoverUrl { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string EraId { get; set; }
        public string? Sources { get; set; }
        public DateTime? PublishedAt { get; set; }
    }

    public class UpdateArticleDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string CoverUrl { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string EraId { get; set; }
        public string? Sources { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}
