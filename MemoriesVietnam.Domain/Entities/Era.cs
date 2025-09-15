using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Common;

namespace MemoriesVietnam.Domain.Entities
{
    public class Era : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public string Description { get; set; } = "";

        public ICollection<Article>? Articles { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
