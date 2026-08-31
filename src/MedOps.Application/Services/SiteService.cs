namespace MedOps.Application.Services;

using MedOps.Domain.Enums;
using MedOps.Domain.ValueObjects;
using MedOps.Domain.Exceptions;
using MedOps.Domain.Interfaces;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Application.Validators;

public class SiteService : ISiteService
{
    private readonly IRepository<Site> _siteRepository;
    private readonly CreateSiteValidator _createValidator;
    private readonly UpdateSiteValidator _updateValidator;

    public SiteService(IRepository<Site> siteRepository, CreateSiteValidator createValidator, UpdateSiteValidator updateValidator)
    {
        _siteRepository = siteRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<List<SiteDto>> GetAllAsync()
    {
        var sites = await _siteRepository.GetAllAsync();
        return sites.Select(s => new SiteDto
        {
            Id = s.Id, Name = s.Name, Description = s.Description, Status = s.Status,
            Address = new AddressDto { Street = s.Address.Street, City = s.Address.City, State = s.Address.State, Country = s.Address.Country, ZipCode = s.Address.ZipCode },
            ContactInfo = new ContactInfoDto { Email = s.ContactInfo.Email, Phone = s.ContactInfo.Phone },
            CreatedBy = s.CreatedBy, CreatedAt = s.CreatedAt, UpdatedAt = s.UpdatedAt
        }).ToList();
    }

    public async Task<SiteDto?> GetByIdAsync(Guid id)
    {
        var site = await _siteRepository.GetByIdAsync(id) ?? throw new SiteNotFoundException(id);
        return new SiteDto
        {
            Id = site.Id, Name = site.Name, Description = site.Description, Status = site.Status,
            Address = new AddressDto { Street = site.Address.Street, City = site.Address.City, State = site.Address.State, Country = site.Address.Country, ZipCode = site.Address.ZipCode },
            ContactInfo = new ContactInfoDto { Email = site.ContactInfo.Email, Phone = site.ContactInfo.Phone },
            CreatedBy = site.CreatedBy, CreatedAt = site.CreatedAt, UpdatedAt = site.UpdatedAt
        };
    }

    public async Task<SiteDto> CreateAsync(CreateSiteDto dto, Guid userId)
    {
        await _createValidator.ValidateAndThrowAsync(dto);
        var address = new Address(dto.Address.Street, dto.Address.City, dto.Address.State, dto.Address.Country, dto.Address.ZipCode);
        var contactInfo = new ContactInfo(dto.ContactInfo.Email, dto.ContactInfo.Phone);
        var site = new Site(dto.Name, dto.Description, address, contactInfo, userId);
        await _siteRepository.AddAsync(site);
        return new SiteDto
        {
            Id = site.Id, Name = site.Name, Description = site.Description, Status = site.Status,
            Address = new AddressDto { Street = site.Address.Street, City = site.Address.City, State = site.Address.State, Country = site.Address.Country, ZipCode = site.Address.ZipCode },
            ContactInfo = new ContactInfoDto { Email = site.ContactInfo.Email, Phone = site.ContactInfo.Phone },
            CreatedBy = site.CreatedBy, CreatedAt = site.CreatedAt, UpdatedAt = site.UpdatedAt
        };
    }

    public async Task<SiteDto> UpdateAsync(Guid id, UpdateSiteDto dto)
    {
        await _updateValidator.ValidateAndThrowAsync(dto);
        var site = await _siteRepository.GetByIdAsync(id) ?? throw new SiteNotFoundException(id);
        var address = new Address(dto.Address.Street, dto.Address.City, dto.Address.State, dto.Address.Country, dto.Address.ZipCode);
        var contactInfo = new ContactInfo(dto.ContactInfo.Email, dto.ContactInfo.Phone);
        site.UpdateDetails(dto.Name, dto.Description, address, contactInfo);
        await _siteRepository.UpdateAsync(site);
        return new SiteDto
        {
            Id = site.Id, Name = site.Name, Description = site.Description, Status = site.Status,
            Address = new AddressDto { Street = site.Address.Street, City = site.Address.City, State = site.Address.State, Country = site.Address.Country, ZipCode = site.Address.ZipCode },
            ContactInfo = new ContactInfoDto { Email = site.ContactInfo.Email, Phone = site.ContactInfo.Phone },
            CreatedBy = site.CreatedBy, CreatedAt = site.CreatedAt, UpdatedAt = site.UpdatedAt
        };
    }

    public async Task DeactivateAsync(Guid id)
    {
        var site = await _siteRepository.GetByIdAsync(id) ?? throw new SiteNotFoundException(id);
        site.Deactivate();
        await _siteRepository.UpdateAsync(site);
    }

    public async Task ActivateAsync(Guid id)
    {
        var site = await _siteRepository.GetByIdAsync(id) ?? throw new SiteNotFoundException(id);
        site.Activate();
        await _siteRepository.UpdateAsync(site);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _siteRepository.DeleteAsync(id);
    }
}