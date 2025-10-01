using MemoriesVietnam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IArticleTagRepository
    {
        Task<IEnumerable<ArticleTag>> GetByArticleIdAsync(string articleId);
        Task<IEnumerable<ArticleTag>> GetByTagIdAsync(string tagId);
    }
}
