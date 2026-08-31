namespace MedOps.Infrastructure.Services;

using System.Threading.Tasks;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TaskStatus = MedOps.Domain.Enums.TaskStatus;

public class DashboardService : IDashboardService
{
    private readonly MedOpsDbContext _context;
    public DashboardService(MedOpsDbContext context) { _context = context; }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var studies = _context.Studies.AsNoTracking();
        var sites = _context.Sites.AsNoTracking();
        var tasks = _context.Tasks.AsNoTracking();
        var requests = _context.Requests.AsNoTracking();
        var departments = _context.Departments.AsNoTracking();

        return new DashboardDto
        {
            Stats = new DashboardStats
            {
                TotalStudies = await studies.CountAsync(),
                ActiveStudies = await studies.CountAsync(s => s.Status == StudyStatus.Active),
                TotalSites = await sites.CountAsync(),
                ActiveSites = await sites.CountAsync(s => s.Status == SiteStatus.Active),
                TotalTasks = await tasks.CountAsync(),
                CompletedTasks = await tasks.CountAsync(t => t.Status == TaskStatus.Completed),
                PendingRequests = await requests.CountAsync(r => r.Status == RequestStatus.Pending),
                TotalDepartments = await departments.CountAsync()
            },
            StudiesByStatus = await studies.GroupBy(s => s.Status).Select(g => new StatusBreakdown { Status = g.Key.ToString(), Count = g.Count() }).ToListAsync(),
            TasksByStatus = await tasks.GroupBy(t => t.Status).Select(g => new StatusBreakdown { Status = g.Key.ToString(), Count = g.Count() }).ToListAsync(),
            RequestsByStatus = await requests.GroupBy(r => r.Status).Select(g => new StatusBreakdown { Status = g.Key.ToString(), Count = g.Count() }).ToListAsync(),
            MonthlyActivity = await studies.GroupBy(s => s.CreatedAt.Month).OrderBy(g => g.Key)
                .Select(g => new MonthlyActivity { Month = g.Key.ToString(), StudiesCreated = g.Count() }).ToListAsync(),
            RecentActivities = await _context.ActivityLogs.AsNoTracking().OrderByDescending(a => a.Timestamp).Take(20)
                .Select(a => new RecentActivityDto { UserName = a.UserName, Action = a.Action, EntityType = a.EntityType, EntityName = a.EntityName, Timestamp = a.Timestamp })
                .ToListAsync(),
            OverdueTasks = await tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value < DateOnly.FromDateTime(DateTime.UtcNow) && t.Status != TaskStatus.Completed && t.Status != TaskStatus.Cancelled)
                .Select(t => new OverdueItemDto { Id = t.Id, Title = t.Title, Status = t.Status.ToString(), DueDate = t.DueDate })
                .ToListAsync(),
            PendingRequests = await requests.Where(r => r.Status == RequestStatus.Pending)
                .Select(r => new OverdueItemDto { Id = r.Id, Title = r.Title, Status = r.Status.ToString() }).ToListAsync()
        };
    }

    public async Task<List<RecentActivityDto>> GetRecentActivityAsync(int limit = 20)
    {
        return await _context.ActivityLogs.AsNoTracking().OrderByDescending(a => a.Timestamp).Take(limit)
            .Select(a => new RecentActivityDto { UserName = a.UserName, Action = a.Action, EntityType = a.EntityType, EntityName = a.EntityName, Timestamp = a.Timestamp })
            .ToListAsync();
    }
}
