using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        Task<IEnumerable<Article>> GetByEraIdAsync(string eraId);
        Task<IEnumerable<Article>> GetByYearRangeAsync(int year);
        Task<Article?> GetBySlugAsync(string slug);
        Task<IEnumerable<Article>> GetPublishedAsync();
    }
}
