namespace MedOps.Api.Controllers;

using MedOps.Application.Interfaces;
using MedOps.Infrastructure.Services;
using MedOps.Application.Common;
using MedOps.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly IActivityLogService _activityLogService;

    public DashboardController(IDashboardService dashboardService, IActivityLogService activityLogService)
    {
        _dashboardService = dashboardService;
        _activityLogService = activityLogService;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboard()
    {
        return Ok(await _dashboardService.GetDashboardAsync());
    }

    [HttpGet("activity")]
    public async Task<ActionResult<PaginatedResult<RecentActivityDto>>> GetActivity([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        return Ok(await _activityLogService.GetRecentActivityAsync(page, pageSize));
    }
}
