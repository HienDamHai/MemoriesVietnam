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
    public class HistoryLogRepository : GenericRepository<HistoryLog>, IHistoryLogRepository
    {
        private readonly AppDbContext _context;

        public HistoryLogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistoryLog>> GetByUserAsync(string userId)
        {
            return await _context.HistoryLogs
                .Where(h => h.UserId == userId)
                .Include(h => h.User)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<HistoryLog>> GetByTargetAsync(string targetId, TargetType targetType)
        {
            return await _context.HistoryLogs
                .Where(h => h.TargetId == targetId && h.TargetType == targetType)
                .Include(h => h.User)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }

        public async Task<HistoryLog?> GetLatestAsync(string userId, string targetId, TargetType targetType)
        {
            return await _context.HistoryLogs
                .Where(h => h.UserId == userId && h.TargetId == targetId && h.TargetType == targetType)
                .OrderByDescending(h => h.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
