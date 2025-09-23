using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId, bool includeRead = true);
        Task<int> CountUnreadAsync(string userId);
        Task<Notification> CreateNotificationAsync(NotifDto.NotificationCreateRequest request);
        Task<bool> MarkAsReadAsync(string notificationId);
        Task<bool> DeleteNotificationAsync(string notificationId);
    }
}
