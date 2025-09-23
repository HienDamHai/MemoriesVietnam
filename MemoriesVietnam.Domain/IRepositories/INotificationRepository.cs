using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId, bool includeRead = true);
        Task<int> CountUnreadAsync(string userId);
    }
}
