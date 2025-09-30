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
    public class BookmarkRepository : GenericRepository<Bookmark>,IBookmarksRepository
    {
        private readonly AppDbContext _context;
        
        public BookmarkRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Bookmark>> GetBookmarkByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            return await _context.Bookmarks
                .Where(b => b.UserId == userId && !b.IsDeleted)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bookmark>> GetBookmarkByTargetAsync(string targetId, TargetType targetType)
        {
            if (string.IsNullOrEmpty(targetId))
                throw new ArgumentException("Target ID cannot be null or empty.", nameof(targetId));

            return await _context.Bookmarks
                .Where(b => b.TargetId == targetId && b.TargetType == targetType && !b.IsDeleted)
                .Include(b => b.User)
                .ToListAsync();
        }
    }
}
