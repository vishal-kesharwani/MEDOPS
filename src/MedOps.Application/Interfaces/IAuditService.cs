namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;
using MedOps.Application.Common;

public interface IAuditService
{
    Task<PaginatedResult<AuditLogDto>> GetAuditLogsAsync(SearchParams search);
    Task<PaginatedResult<AuditLogDto>> GetEntityAuditLogsAsync(string entityName, Guid entityId, int page = 1, int pageSize = 20);
    Task LogAsync(string entityName, Guid entityId, string action, Guid userId, string userName, 
        string? oldValues = null, string? newValues = null, string? description = null);
}
