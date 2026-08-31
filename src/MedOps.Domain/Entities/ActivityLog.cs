namespace MedOps.Domain.Entities;

public class ActivityLog
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;
    public string EntityType { get; private set; } = string.Empty;
    public Guid? EntityId { get; private set; }
    public string? EntityName { get; private set; }
    public string? Details { get; private set; }
    public DateTime Timestamp { get; private set; }
    public bool IsRead { get; private set; }

    private ActivityLog() { }

    public ActivityLog(Guid userId, string userName, string action, string entityType,
        Guid? entityId = null, string? entityName = null, string? details = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        UserName = userName;
        Action = action;
        EntityType = entityType;
        EntityId = entityId;
        EntityName = entityName;
        Details = details;
        Timestamp = DateTime.UtcNow;
        IsRead = false;
    }

    public void MarkAsRead() { IsRead = true; }
}
