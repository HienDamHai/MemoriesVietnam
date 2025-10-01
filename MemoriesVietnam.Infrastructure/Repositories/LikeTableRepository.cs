using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
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
    public class LikeTableRepository : GenericRepository<LikeTable>, ILikeTableRepository
    {
        private readonly AppDbContext _context;

        public LikeTableRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LikeTable>> GetByUserIdAsync(string userId)
        {
            return await _context.LikeTables
                .Where(l => l.UserId == userId && !l.IsDeleted)
                .Include(l => l.User)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<LikeTable>> GetByTargetAsync(string targetId, TargetType targetType)
        {
            return await _context.LikeTables
                .Where(l => l.TargetId == targetId && l.TargetType == targetType && !l.IsDeleted)
                .Include(l => l.User)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<LikeTable?> GetUserLikeAsync(string userId, string targetId, TargetType targetType)
        {
            return await _context.LikeTables
                .FirstOrDefaultAsync(l => l.UserId == userId && l.TargetId == targetId && l.TargetType == targetType && !l.IsDeleted);
        }
    }
}
