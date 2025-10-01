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
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        private readonly AppDbContext _context;

        public ArticleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetByEraIdAsync(string eraId)
        {
            return await _context.Articles
                .Where(a => a.EraId == eraId && !a.IsDeleted)
                .Include(a => a.Era)
                .Include(a => a.ArticleAudios)
                .Include(a => a.ArticleTags)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetByYearRangeAsync(int year)
        {
            return await _context.Articles
                .Where(a => !a.IsDeleted && a.YearStart <= year && a.YearEnd >= year)
                .Include(a => a.Era)
                .ToListAsync();
        }

        public async Task<Article?> GetBySlugAsync(string slug)
        {
            return await _context.Articles
                .Where(a => a.Slug == slug && !a.IsDeleted)
                .Include(a => a.Era)
                .Include(a => a.ArticleAudios)
                .Include(a => a.ArticleTags)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Article>> GetPublishedAsync()
        {
            return await _context.Articles
                .Where(a => !a.IsDeleted && a.PublishedAt != null)
                .Include(a => a.Era)
                .OrderByDescending(a => a.PublishedAt)
                .ToListAsync();
        }
    }
}
