using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class ArticleTagDto
    {
        public string Id { get; set; }
        public string ArticleId { get; set; }
        public string TagId { get; set; }
    }

    public class CreateArticleTagDto
    {
        public string ArticleId { get; set; }
        public string TagId { get; set; }
    }

    public class UpdateArticleTagDto
    {
        public string ArticleId { get; set; }
        public string TagId { get; set; }
    }

}
