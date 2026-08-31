namespace MedOps.Infrastructure.Services;

using System.Threading.Tasks;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Notification = MedOps.Domain.Entities.Notification;

public class NotificationService : INotificationService
{
    private readonly MedOpsDbContext _context;
    public NotificationService(MedOpsDbContext context) { _context = context; }

    public async Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId, bool unreadOnly = false)
    {
        var query = _context.Notifications.Where(n => n.UserId == userId);
        if (unreadOnly) query = query.Where(n => !n.IsRead);
        return await query.OrderByDescending(n => n.CreatedAt).Take(50)
            .Select(n => new NotificationDto
            {
                Id = n.Id, Title = n.Title, Message = n.Message, Type = n.Type,
                Link = n.Link, IsRead = n.IsRead, CreatedAt = n.CreatedAt
            }).ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(Guid userId) => await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);

    public async Task CreateAsync(Guid userId, string title, string message, string type = "Info", string? link = null)
    {
        _context.Notifications.Add(new Notification(userId, title, message, type, link));
        await _context.SaveChangesAsync();
    }

    public async Task MarkAsReadAsync(Guid notificationId, Guid userId)
    {
        var n = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId && x.UserId == userId);
        if (n != null) { n.MarkAsRead(); await _context.SaveChangesAsync(); }
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        var unread = await _context.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
        foreach (var n in unread) n.MarkAsRead();
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid notificationId, Guid userId)
    {
        var n = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId && x.UserId == userId);
        if (n != null) { _context.Notifications.Remove(n); await _context.SaveChangesAsync(); }
    }
}
