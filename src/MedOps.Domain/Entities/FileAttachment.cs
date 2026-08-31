namespace MedOps.Domain.Entities;

public class FileAttachment
{
    public Guid Id { get; private set; }
    public string EntityType { get; private set; } = string.Empty;
    public Guid EntityId { get; private set; }
    public Guid UploadedBy { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string OriginalFileName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long FileSize { get; private set; }
    public string StoragePath { get; private set; } = string.Empty;
    public DateTime UploadedAt { get; private set; }

    private FileAttachment() { }

    public FileAttachment(string entityType, Guid entityId, Guid uploadedBy, string fileName,
        string originalFileName, string contentType, long fileSize, string storagePath)
    {
        Id = Guid.NewGuid();
        EntityType = entityType;
        EntityId = entityId;
        UploadedBy = uploadedBy;
        FileName = fileName;
        OriginalFileName = originalFileName;
        ContentType = contentType;
        FileSize = fileSize;
        StoragePath = storagePath;
        UploadedAt = DateTime.UtcNow;
    }
}
