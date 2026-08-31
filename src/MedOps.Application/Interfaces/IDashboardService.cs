namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
    Task<List<RecentActivityDto>> GetRecentActivityAsync(int limit = 20);
}
