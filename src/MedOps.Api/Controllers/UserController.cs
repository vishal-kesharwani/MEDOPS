namespace MedOps.Api.Controllers;

using MedOps.Application.DTOs;
using MedOps.Domain.Entities;
using MedOps.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public UserController(UserManager<ApplicationUser> userManager) { _userManager = userManager; }

    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        var user = await _userManager.FindByIdAsync(UserId.ToString());
        if (user == null) return NotFound();
        return Ok(new UserProfileDto
        {
            Id = user.Id, Email = user.Email!, FirstName = user.FirstName, LastName = user.LastName,
            CreatedAt = user.CreatedAt, IsActive = user.IsActive
        });
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(UserId.ToString());
        if (user == null) return NotFound();
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<UserProfileDto>>> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        return Ok(users.Select(u => new UserProfileDto
        {
            Id = u.Id, Email = u.Email!, FirstName = u.FirstName, LastName = u.LastName,
            CreatedAt = u.CreatedAt, IsActive = u.IsActive
        }).ToList());
    }
}
