namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;

public interface ISiteService
{
    Task<List<SiteDto>> GetAllAsync();
    Task<SiteDto?> GetByIdAsync(Guid id);
    Task<SiteDto> CreateAsync(CreateSiteDto dto, Guid userId);
    Task<SiteDto> UpdateAsync(Guid id, UpdateSiteDto dto);
    Task DeactivateAsync(Guid id);
    Task ActivateAsync(Guid id);
    Task DeleteAsync(Guid id);
}