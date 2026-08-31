namespace MedOps.Api.Controllers;

using MedOps.Application.Interfaces;
using MedOps.Application.Common;
using MedOps.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditController : ControllerBase
{
    private readonly IAuditService _auditService;
    public AuditController(IAuditService auditService) { _auditService = auditService; }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<AuditLogDto>>> GetAll([FromQuery] SearchParams search)
        => Ok(await _auditService.GetAuditLogsAsync(search));

    [HttpGet("{entityType}/{entityId}")]
    public async Task<ActionResult<PaginatedResult<AuditLogDto>>> GetEntityLogs(string entityType, Guid entityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _auditService.GetEntityAuditLogsAsync(entityType, entityId, page, pageSize));
}
