namespace MedOps.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; private set; }
    public string EntityName { get; private set; } = string.Empty;
    public Guid EntityId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public Guid UserId { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public DateTime Timestamp { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }
    public string? Description { get; private set; }

    private AuditLog() { }

    public AuditLog(string entityName, Guid entityId, string action, Guid userId, string userName,
        string? oldValues = null, string? newValues = null, string? description = null)
    {
        Id = Guid.NewGuid();
        EntityName = entityName;
        EntityId = entityId;
        Action = action;
        UserId = userId;
        UserName = userName;
        Timestamp = DateTime.UtcNow;
        OldValues = oldValues;
        NewValues = newValues;
        Description = description;
    }
}
