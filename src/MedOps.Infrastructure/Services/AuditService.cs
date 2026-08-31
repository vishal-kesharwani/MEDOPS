namespace MedOps.Infrastructure.Services;

using System.Threading.Tasks;
using MedOps.Application.Common;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AuditLog = MedOps.Domain.Entities.AuditLog;

public class AuditService : IAuditService
{
    private readonly MedOpsDbContext _context;
    public AuditService(MedOpsDbContext context) { _context = context; }

    public async Task<PaginatedResult<AuditLogDto>> GetAuditLogsAsync(SearchParams search)
    {
        var query = _context.AuditLogs.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(search.Search))
        {
            var term = search.Search.ToLower();
            query = query.Where(a => a.EntityName.ToLower().Contains(term) || a.UserName.ToLower().Contains(term) || a.Action.ToLower().Contains(term));
        }
        query = search.SortBy?.ToLower() switch
        {
            "entityname" => search.SortDescending ? query.OrderByDescending(a => a.EntityName) : query.OrderBy(a => a.EntityName),
            "action" => search.SortDescending ? query.OrderByDescending(a => a.Action) : query.OrderBy(a => a.Action),
            "username" => search.SortDescending ? query.OrderByDescending(a => a.UserName) : query.OrderBy(a => a.UserName),
            _ => query.OrderByDescending(a => a.Timestamp)
        };
        var totalCount = await query.CountAsync();
        var items = await query.Skip((search.Page - 1) * search.PageSize).Take(search.PageSize)
            .Select(a => new AuditLogDto
            {
                Id = a.Id, EntityName = a.EntityName, EntityId = a.EntityId, Action = a.Action,
                UserId = a.UserId, UserName = a.UserName, Timestamp = a.Timestamp,
                OldValues = a.OldValues, NewValues = a.NewValues, Description = a.Description
            }).ToListAsync();
        return new PaginatedResult<AuditLogDto> { Items = items, TotalCount = totalCount, Page = search.Page, PageSize = search.PageSize };
    }

    public async Task<PaginatedResult<AuditLogDto>> GetEntityAuditLogsAsync(string entityName, Guid entityId, int page = 1, int pageSize = 20)
    {
        var query = _context.AuditLogs.AsNoTracking()
            .Where(a => a.EntityName == entityName && a.EntityId == entityId)
            .OrderByDescending(a => a.Timestamp);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(a => new AuditLogDto
            {
                Id = a.Id, EntityName = a.EntityName, EntityId = a.EntityId, Action = a.Action,
                UserId = a.UserId, UserName = a.UserName, Timestamp = a.Timestamp,
                OldValues = a.OldValues, NewValues = a.NewValues, Description = a.Description
            }).ToListAsync();
        return new PaginatedResult<AuditLogDto> { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize };
    }

    public async Task LogAsync(string entityName, Guid entityId, string action, Guid userId, string userName,
        string? oldValues = null, string? newValues = null, string? description = null)
    {
        var log = new AuditLog(entityName, entityId, action, userId, userName, oldValues, newValues, description);
        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
