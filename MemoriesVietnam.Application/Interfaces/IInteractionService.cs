using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Models.Entities;

namespace MemoriesVietnam.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetByTargetAsync(string targetId, CommentTargetType targetType);
        Task<CommentDto> CreateAsync(string userId, CreateCommentDto dto);
        Task<CommentDto?> UpdateAsync(string id, string userId, UpdateCommentDto dto);
        Task<bool> DeleteAsync(string id, string userId);
    }

    public interface ILikeService
    {
        Task<LikeResponseDto> ToggleAsync(string userId, LikeToggleDto dto);
        Task<LikeStatusDto> GetStatusAsync(string targetId, TargetType targetType, string? userId = null);
    }

    public interface IBookmarkService
    {
        Task<List<BookmarkDto>> GetMyBookmarksAsync(string userId);
        Task<BookmarkResponseDto> ToggleAsync(string userId, BookmarkToggleDto dto);
        Task<BookmarkStatusDto> CheckAsync(string targetId, BookmarkTargetType targetType, string userId);
    }

    public interface IHistoryService
    {
        Task<List<HistoryLogDto>> GetMyHistoryAsync(string userId);
        Task<HistoryLogDto> CreateOrUpdateAsync(string userId, CreateHistoryLogDto dto);
    }

    public interface INotificationService
    {
        Task<List<NotificationDto>> GetMyNotificationsAsync(string userId);
        Task<bool> MarkAsReadAsync(string id, string userId);
        Task<bool> MarkAllAsReadAsync(string userId);
        Task CreateNotificationAsync(string userId, string type, string content, string? targetId = null, NotificationTargetType? targetType = null);
    }
}
