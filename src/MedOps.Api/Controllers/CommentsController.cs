namespace MedOps.Api.Controllers;

using MedOps.Application.Interfaces;
using MedOps.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private string UserName => User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";

    public CommentsController(ICommentService commentService) { _commentService = commentService; }

    [HttpGet("{entityType}/{entityId}")]
    public async Task<ActionResult<List<CommentDto>>> Get(string entityType, Guid entityId)
        => Ok(await _commentService.GetCommentsAsync(entityType, entityId));

    [HttpPost("{entityType}/{entityId}")]
    public async Task<ActionResult<CommentDto>> Create(string entityType, Guid entityId, [FromBody] CreateCommentDto dto)
    {
        var comment = await _commentService.AddCommentAsync(entityType, entityId, UserId, UserName, dto);
        return CreatedAtAction(nameof(Get), new { entityType, entityId }, comment);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> Update(Guid id, [FromBody] CreateCommentDto dto)
        => Ok(await _commentService.UpdateCommentAsync(id, UserId, dto.Content));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    { await _commentService.DeleteCommentAsync(id, UserId); return NoContent(); }
}
