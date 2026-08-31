namespace MedOps.Api.Controllers;

using MedOps.Application.Interfaces;
using MedOps.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public FilesController(IFileService fileService) { _fileService = fileService; }

    [HttpGet("{entityType}/{entityId}")]
    public async Task<ActionResult<List<FileAttachmentDto>>> Get(string entityType, Guid entityId)
        => Ok(await _fileService.GetAttachmentsAsync(entityType, entityId));

    [HttpPost("{entityType}/{entityId}")]
    public async Task<ActionResult<FileAttachmentDto>> Upload(string entityType, Guid entityId, IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var result = await _fileService.UploadAsync(entityType, entityId, UserId, stream, file.FileName, file.ContentType, file.Length);
        return Ok(result);
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(Guid id)
    {
        var (stream, contentType, fileName) = await _fileService.DownloadAsync(id);
        return File(stream, contentType, fileName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    { await _fileService.DeleteAsync(id, UserId); return NoContent(); }
}
