using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Interfaces;
using MemoriesVietnam.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace MemoriesVietnam.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(bool includeRead = true)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (userId == null) return Unauthorized();

            var notifications = await _notificationService.GetUserNotificationsAsync(userId);

            var response = notifications.Select(n => new NotifDto.NotificationResponse
            {
                Id = n.Id,
                UserId = n.UserId,
                Type = n.Type,
                Content = n.Content,
                TargetId = n.TargetId,
                TargetType = n.TargetType,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });

            return Ok(response);
        }

        [HttpGet("unread/count")]
        public async Task<IActionResult> CountUnread()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (userId == null) return Unauthorized();
            var count = await _notificationService.CountUnreadAsync(userId);
            return Ok(new { count });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create([FromBody] NotifDto.NotificationCreateRequest notification)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdNotification = await _notificationService.CreateNotificationAsync(notification);

            var response = new NotifDto.NotificationResponse
            {
                Id = createdNotification.Id,
                UserId = createdNotification.UserId,
                Type = createdNotification.Type,
                Content = createdNotification.Content,
                TargetId = createdNotification.TargetId,
                TargetType = createdNotification.TargetType,
                IsRead = createdNotification.IsRead,
                CreatedAt = createdNotification.CreatedAt
            };

            return CreatedAtAction(nameof(GetAll), new { id = response.Id }, response);
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            var success = await _notificationService.MarkAsReadAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
