using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IRepositories;
using MemoriesVietnam.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Infrastructure.Repositories
{
    public class ArticleTagRepository : IArticleTagRepository
    {
        private readonly AppDbContext _context;

        public ArticleTagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleTag>> GetByArticleIdAsync(string articleId)
        {
            if (string.IsNullOrEmpty(articleId))
                throw new ArgumentException("Article ID cannot be null or empty", nameof(articleId));

            return await _context.ArticleTags
                .Where(at => at.ArticleId == articleId && !at.IsDeleted)
                .Include(at => at.Tag)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArticleTag>> GetByTagIdAsync(string tagId)
        {
            if (string.IsNullOrEmpty(tagId))
                throw new ArgumentException("Tag ID cannot be null or empty", nameof(tagId));

            return await _context.ArticleTags
                .Where(at => at.TagId == tagId && !at.IsDeleted)
                .Include(at => at.Article)
                .ToListAsync();
        }
    }
}
