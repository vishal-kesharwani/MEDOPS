using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MedOps.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SitesController : ControllerBase
{
    private readonly ISiteService _siteService;

    public SitesController(ISiteService siteService)
    {
        _siteService = siteService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SiteDto>>> GetAll() => Ok(await _siteService.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SiteDto>> GetById(Guid id)
    {
        var site = await _siteService.GetByIdAsync(id);
        return site is null ? NotFound() : Ok(site);
    }

    [HttpPost]
    public async Task<ActionResult<SiteDto>> Create([FromBody] CreateSiteDto dto)
    {
        var userId = Guid.NewGuid();
        var site = await _siteService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = site.Id }, site);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SiteDto>> Update(Guid id, [FromBody] UpdateSiteDto dto)
    {
        var site = await _siteService.UpdateAsync(id, dto);
        return Ok(site);
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _siteService.DeactivateAsync(id);
        return Ok();
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        await _siteService.ActivateAsync(id);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _siteService.DeleteAsync(id);
        return NoContent();
    }
}