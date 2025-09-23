using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Interfaces;
using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using MemoriesVietnam.Domain.IRepositories;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IUnitOfWork unitOfWork, INotificationRepository notificationRepository)
        {
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId, bool includeRead = true)
        {
            return await _notificationRepository.GetUserNotificationsAsync(userId, includeRead);
        }

        public async Task<int> CountUnreadAsync(string userId)
        {
            return await _notificationRepository.CountUnreadAsync(userId);
        }

        public async Task<Notification> CreateNotificationAsync(NotifDto.NotificationCreateRequest request)
        {
            var notification = new Notification
            {
                UserId = request.UserId,
                Type = request.Type,
                Content = request.Content,
                TargetId = request.TargetId,
                TargetType = request.TargetType,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> MarkAsReadAsync(string notificationId)
        {
            var notif = await _notificationRepository.GetByIdAsync(notificationId);
            if (notif == null) return false;

            notif.IsRead = true;
            _notificationRepository.Update(notif);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(string notificationId)
        {
            var notif = await _notificationRepository.GetByIdAsync(notificationId);
            if (notif == null) return false;

            _notificationRepository.Remove(notif);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
