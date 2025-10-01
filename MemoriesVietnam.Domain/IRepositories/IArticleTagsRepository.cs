using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IArticleTagsRepository : IGenericRepository<ArticleTag>
    {
        Task<IEnumerable<ArticleTag>> GetByArticleIdAsync(string articleId);
        Task<IEnumerable<ArticleTag>> GetByTagIdAsync(string tagId);
    }
}
