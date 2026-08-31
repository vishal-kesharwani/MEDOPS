namespace MedOps.Api.Services;

using MedOps.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

public interface INotificationHubService
{
    Task NotifyUserAsync(string userId, string title, string message, string type = "Info");
    Task BroadcastActivityAsync(string action, string entityType, string? entityName = null);
}

public class NotificationHubService : INotificationHubService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHubService(IHubContext<NotificationHub> hubContext) { _hubContext = hubContext; }

    public async Task NotifyUserAsync(string userId, string title, string message, string type = "Info")
    {
        await _hubContext.Clients.Group($"user_{userId}").SendAsync("ReceiveNotification", new { title, message, type, createdAt = DateTime.UtcNow });
    }

    public async Task BroadcastActivityAsync(string action, string entityType, string? entityName = null)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveActivityUpdate", new { action, entityType, entityName, timestamp = DateTime.UtcNow });
    }
}