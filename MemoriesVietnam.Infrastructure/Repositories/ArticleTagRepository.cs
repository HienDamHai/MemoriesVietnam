using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IRepositories;
using MemoriesVietnam.Infrastructure.Basic;
using MemoriesVietnam.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Infrastructure.Repositories
{
    public class ArticleTagRepository : GenericRepository<ArticleTag>, IArticleTagRepository
    {
        private readonly AppDbContext _context;

        public ArticleTagRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleTag>> GetByArticleIdAsync(string articleId)
        {
            return await _context.ArticleTags
                .Where(at => at.ArticleId == articleId && !at.IsDeleted)
                .Include(at => at.Tag)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArticleTag>> GetByTagIdAsync(string tagId)
        {
            return await _context.ArticleTags
                .Where(at => at.TagId == tagId && !at.IsDeleted)
                .Include(at => at.Article)
                .ToListAsync();
        }
    }
}
