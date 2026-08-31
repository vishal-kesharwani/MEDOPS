namespace MedOps.Domain.Entities;

public class Comment
{
    public Guid Id { get; private set; }
    public string EntityType { get; private set; } = string.Empty;
    public Guid EntityId { get; private set; }
    public Guid UserId { get; private set; }
    public string UserName { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    private Comment() { }

    public Comment(string entityType, Guid entityId, Guid userId, string userName, string content)
    {
        Id = Guid.NewGuid();
        EntityType = entityType;
        EntityId = entityId;
        UserId = userId;
        UserName = userName;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateContent(string content)
    {
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete() { IsDeleted = true; UpdatedAt = DateTime.UtcNow; }
}
