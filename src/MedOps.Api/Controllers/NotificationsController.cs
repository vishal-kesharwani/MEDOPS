namespace MedOps.Api.Controllers;

using MedOps.Application.Interfaces;
using MedOps.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public NotificationsController(INotificationService notificationService) { _notificationService = notificationService; }

    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> Get([FromQuery] bool unreadOnly = false)
        => Ok(await _notificationService.GetUserNotificationsAsync(UserId, unreadOnly));

    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount()
        => Ok(await _notificationService.GetUnreadCountAsync(UserId));

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    { await _notificationService.MarkAsReadAsync(id, UserId); return NoContent(); }

    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    { await _notificationService.MarkAllAsReadAsync(UserId); return NoContent(); }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    { await _notificationService.DeleteAsync(id, UserId); return NoContent(); }
}
