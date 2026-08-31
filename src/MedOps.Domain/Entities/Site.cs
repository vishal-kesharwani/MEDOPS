namespace MedOps.Domain.Entities;

using MedOps.Domain.Enums;
using MedOps.Domain.ValueObjects;
using MedOps.Domain.Exceptions;

public class Site
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public SiteStatus Status { get; private set; }
    public Address Address { get; private set; } = new();
    public ContactInfo ContactInfo { get; private set; } = new();
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ICollection<StudySite> StudySites { get; private set; } = new List<StudySite>();

    private Site() { }

    public Site(string name, string description, Address address, ContactInfo contactInfo, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        CreatedBy = createdBy;
        Status = SiteStatus.Active;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (Status != SiteStatus.Active)
            throw new DomainException("Only active sites can be deactivated.", "INVALID_SITE_TRANSITION");
        Status = SiteStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status != SiteStatus.Inactive)
            throw new DomainException("Only inactive sites can be activated.", "INVALID_SITE_TRANSITION");
        Status = SiteStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description, Address address, ContactInfo contactInfo)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        UpdatedAt = DateTime.UtcNow;
    }
}