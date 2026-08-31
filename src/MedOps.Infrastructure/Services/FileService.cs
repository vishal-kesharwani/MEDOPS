namespace MedOps.Infrastructure.Services;

using System.Threading.Tasks;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Domain.Exceptions;
using MedOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FileAttachment = MedOps.Domain.Entities.FileAttachment;

public class FileService : IFileService
{
    private readonly MedOpsDbContext _context;
    private readonly string _uploadPath;

    public FileService(MedOpsDbContext context, IConfiguration config)
    {
        _context = context;
        _uploadPath = config.GetValue<string>("FileStorage:UploadPath") ?? Path.Combine(AppContext.BaseDirectory, "uploads");
        if (!Directory.Exists(_uploadPath)) Directory.CreateDirectory(_uploadPath);
    }

    public async Task<List<FileAttachmentDto>> GetAttachmentsAsync(string entityType, Guid entityId)
    {
        return await _context.FileAttachments
            .Where(f => f.EntityType == entityType && f.EntityId == entityId)
            .OrderByDescending(f => f.UploadedAt)
            .Select(f => new FileAttachmentDto
            {
                Id = f.Id, EntityType = f.EntityType, EntityId = f.EntityId,
                UploadedBy = f.UploadedBy, FileName = f.FileName, OriginalFileName = f.OriginalFileName,
                ContentType = f.ContentType, FileSize = f.FileSize, UploadedAt = f.UploadedAt
            }).ToListAsync();
    }

    public async Task<FileAttachmentDto> UploadAsync(string entityType, Guid entityId, Guid userId, Stream fileStream, string fileName, string contentType, long fileSize)
    {
        var entityFolder = Path.Combine(_uploadPath, entityType, entityId.ToString());
        if (!Directory.Exists(entityFolder)) Directory.CreateDirectory(entityFolder);
        var storedFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var filePath = Path.Combine(entityFolder, storedFileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
            await fileStream.CopyToAsync(stream);
        var attachment = new FileAttachment(entityType, entityId, userId, storedFileName, fileName, contentType, fileSize, filePath);
        _context.FileAttachments.Add(attachment);
        await _context.SaveChangesAsync();
        return new FileAttachmentDto
        {
            Id = attachment.Id, EntityType = attachment.EntityType, EntityId = attachment.EntityId,
            UploadedBy = attachment.UploadedBy, FileName = attachment.FileName, OriginalFileName = attachment.OriginalFileName,
            ContentType = attachment.ContentType, FileSize = attachment.FileSize, UploadedAt = attachment.UploadedAt
        };
    }

    public async Task<(Stream stream, string contentType, string fileName)> DownloadAsync(Guid fileId)
    {
        var attachment = await _context.FileAttachments.FindAsync(fileId) ?? throw new DomainException("File not found", "FILE_NOT_FOUND");
        if (!System.IO.File.Exists(attachment.StoragePath)) throw new DomainException("File not found on disk", "FILE_MISSING");
        return (new FileStream(attachment.StoragePath, FileMode.Open, FileAccess.Read), attachment.ContentType, attachment.OriginalFileName);
    }

    public async Task DeleteAsync(Guid fileId, Guid userId)
    {
        var attachment = await _context.FileAttachments.FindAsync(fileId) ?? throw new DomainException("File not found", "FILE_NOT_FOUND");
        if (File.Exists(attachment.StoragePath)) File.Delete(attachment.StoragePath);
        _context.FileAttachments.Remove(attachment);
        await _context.SaveChangesAsync();
    }
}
