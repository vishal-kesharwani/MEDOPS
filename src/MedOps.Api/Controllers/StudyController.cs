using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MedOps.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudiesController : ControllerBase
{
    private readonly IStudyService _studyService;

    public StudiesController(IStudyService studyService)
    {
        _studyService = studyService;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudyDto>>> GetAll() => Ok(await _studyService.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StudyDto>> GetById(Guid id)
    {
        var study = await _studyService.GetByIdAsync(id);
        return study is null ? NotFound() : Ok(study);
    }

    [HttpPost]
    public async Task<ActionResult<StudyDto>> Create([FromBody] CreateStudyDto dto)
    {
        var userId = Guid.NewGuid();
        var study = await _studyService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = study.Id }, study);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<StudyDto>> Update(Guid id, [FromBody] UpdateStudyDto dto)
    {
        var study = await _studyService.UpdateAsync(id, dto);
        return Ok(study);
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, [FromBody] DateOnly startDate, DateOnly endDate)
    {
        await _studyService.ActivateAsync(id, startDate, endDate);
        return Ok();
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        await _studyService.CompleteAsync(id);
        return Ok();
    }

    [HttpPost("{id:guid}/suspend")]
    public async Task<IActionResult> Suspend(Guid id)
    {
        await _studyService.SuspendAsync(id);
        return Ok();
    }

    [HttpPost("{id:guid}/terminate")]
    public async Task<IActionResult> Terminate(Guid id)
    {
        await _studyService.TerminateAsync(id);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _studyService.DeleteAsync(id);
        return NoContent();
    }
}