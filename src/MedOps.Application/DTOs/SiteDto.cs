namespace MedOps.Application.DTOs;

using MedOps.Domain.Enums;
using MedOps.Domain.ValueObjects;

public class SiteDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SiteStatus Status { get; set; }
    public AddressDto Address { get; set; } = new();
    public ContactInfoDto ContactInfo { get; set; } = new();
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}

public class ContactInfoDto
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class CreateSiteDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = new();
    public ContactInfoDto ContactInfo { get; set; } = new();
}

public class UpdateSiteDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = new();
    public ContactInfoDto ContactInfo { get; set; } = new();
}