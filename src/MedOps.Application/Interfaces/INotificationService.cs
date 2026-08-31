namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;

public interface INotificationService
{
    Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId, bool unreadOnly = false);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task CreateAsync(Guid userId, string title, string message, string type = "Info", string? link = null);
    Task MarkAsReadAsync(Guid notificationId, Guid userId);
    Task MarkAllAsReadAsync(Guid userId);
    Task DeleteAsync(Guid notificationId, Guid userId);
}
