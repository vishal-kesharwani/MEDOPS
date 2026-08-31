namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;

public interface IFileService
{
    Task<List<FileAttachmentDto>> GetAttachmentsAsync(string entityType, Guid entityId);
    Task<FileAttachmentDto> UploadAsync(string entityType, Guid entityId, Guid userId, Stream fileStream, string fileName, string contentType, long fileSize);
    Task<(Stream stream, string contentType, string fileName)> DownloadAsync(Guid fileId);
    Task DeleteAsync(Guid fileId, Guid userId);
}
