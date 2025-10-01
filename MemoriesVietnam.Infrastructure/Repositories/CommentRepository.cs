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
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetByTargetAsync(string targetId, TargetType targetType)
        {
            return await _context.Comments
                .Where(c => c.TargetId == targetId && c.TargetType == targetType && !c.IsDeleted && c.ParentId == null)
                .Include(c => c.User)
                .Include(c => c.Replies)
                .ThenInclude(r => r.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByUserAsync(string userId)
        {
            return await _context.Comments
                .Where(c => c.UserId == userId && !c.IsDeleted)
                .Include(c => c.User)        
                .Include(c => c.Replies)     
                .ThenInclude(r => r.User)   
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }


        public async Task<IEnumerable<Comment>> GetRepliesAsync(string parentId)
        {
            return await _context.Comments
                .Where(c => c.ParentId == parentId && !c.IsDeleted)
                .Include(c => c.User)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
