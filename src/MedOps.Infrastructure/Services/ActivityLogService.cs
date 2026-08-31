namespace MedOps.Infrastructure.Services;

using System.Threading.Tasks;
using MedOps.Application.Common;
using MedOps.Application.DTOs;
using MedOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ActivityLog = MedOps.Domain.Entities.ActivityLog;

public interface IActivityLogService
{
    Task LogAsync(Guid userId, string userName, string action, string entityType, Guid? entityId = null, string? entityName = null, string? details = null);
    Task<PaginatedResult<RecentActivityDto>> GetRecentActivityAsync(int page = 1, int pageSize = 20);
}

public class ActivityLogService : IActivityLogService
{
    private readonly MedOpsDbContext _context;
    public ActivityLogService(MedOpsDbContext context) { _context = context; }

    public async Task LogAsync(Guid userId, string userName, string action, string entityType, Guid? entityId = null, string? entityName = null, string? details = null)
    {
        _context.ActivityLogs.Add(new ActivityLog(userId, userName, action, entityType, entityId, entityName, details));
        await _context.SaveChangesAsync();
    }

    public async Task<PaginatedResult<RecentActivityDto>> GetRecentActivityAsync(int page = 1, int pageSize = 20)
    {
        var query = _context.ActivityLogs.AsNoTracking().OrderByDescending(a => a.Timestamp);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(a => new RecentActivityDto { UserName = a.UserName, Action = a.Action, EntityType = a.EntityType, EntityName = a.EntityName, Timestamp = a.Timestamp })
            .ToListAsync();
        return new PaginatedResult<RecentActivityDto> { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize };
    }
}
