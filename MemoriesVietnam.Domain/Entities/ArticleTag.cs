using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Common;

namespace MemoriesVietnam.Domain.Entities
{
    public class ArticleTag : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ArticleId { get; set; }
        public Article Article { get; set; }
        public string TagId { get; set; }
        public Tag Tag { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
