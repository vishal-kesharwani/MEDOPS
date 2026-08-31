namespace MedOps.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string? Link { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Notification() { }

    public Notification(Guid userId, string title, string message, string type = "Info", string? link = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Message = message;
        Type = type;
        Link = link;
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead() { IsRead = true; }
}
