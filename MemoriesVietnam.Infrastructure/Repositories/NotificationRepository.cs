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
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId, bool includeRead = true)
        {
            var query = _context.Notifications
                .Where(n => n.UserId == userId && !n.IsDeleted);

            if (!includeRead)
                query = query.Where(n => !n.IsRead);

            query = query.OrderByDescending(n => n.CreatedAt);

            return await query.ToListAsync();
        }

        public async Task<int> CountUnreadAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead && !n.IsDeleted);
        }
    }
}
