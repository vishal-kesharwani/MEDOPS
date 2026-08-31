using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MedOps.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RequestsController : ControllerBase
{
    private readonly IRequestService _requestService;

    public RequestsController(IRequestService requestService)
    {
        _requestService = requestService;
    }

    [HttpGet]
    public async Task<ActionResult<List<RequestDto>>> GetAll() => Ok(await _requestService.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestDto>> GetById(Guid id)
    {
        var request = await _requestService.GetByIdAsync(id);
        return request is null ? NotFound() : Ok(request);
    }

    [HttpPost]
    public async Task<ActionResult<RequestDto>> Create([FromBody] CreateRequestDto dto)
    {
        var userId = Guid.NewGuid();
        var request = await _requestService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var userId = Guid.NewGuid();
        await _requestService.ApproveAsync(id, userId);
        return Ok();
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] string comment)
    {
        var userId = Guid.NewGuid();
        await _requestService.RejectAsync(id, userId, comment);
        return Ok();
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _requestService.CancelAsync(id);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _requestService.DeleteAsync(id);
        return NoContent();
    }
}