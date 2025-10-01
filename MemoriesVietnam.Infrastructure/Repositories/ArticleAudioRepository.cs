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
    public class ArticleAudioRepository : GenericRepository<ArticleAudio>, IArticleAudioRepository
    {
        private readonly AppDbContext _context;

        public ArticleAudioRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleAudio>> GetByArticleIdAsync(string articleId)
        {
            return await _context.ArticleAudios
                .Where(a => a.ArticleId == articleId && !a.IsDeleted)
                .Include(a => a.Article)
                .Include(a => a.CreatedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArticleAudio>> GetByUserIdAsync(string userId)
        {
            return await _context.ArticleAudios
                .Where(a => a.CreatedBy == userId && !a.IsDeleted)
                .Include(a => a.Article)
                .ToListAsync();
        }
    }
}
