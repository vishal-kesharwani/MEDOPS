namespace MedOps.Domain.Entities;

using MedOps.Domain.Enums;
using MedOps.Domain.Exceptions;

public class StudySite
{
    public Guid Id { get; private set; }
    public Guid StudyId { get; private set; }
    public Guid SiteId { get; private set; }
    public StudySiteStatus Status { get; private set; }
    public string Role { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private StudySite() { }

    public StudySite(Guid studyId, Guid siteId, string role)
    {
        Id = Guid.NewGuid();
        StudyId = studyId;
        SiteId = siteId;
        Role = role ?? throw new ArgumentNullException(nameof(role));
        Status = StudySiteStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status != StudySiteStatus.Pending)
            throw new DomainException("Only pending study sites can be activated.", "INVALID_STUDY_SITE_TRANSITION");
        Status = StudySiteStatus.Active;
    }

    public void Deactivate()
    {
        if (Status != StudySiteStatus.Active)
            throw new DomainException("Only active study sites can be deactivated.", "INVALID_STUDY_SITE_TRANSITION");
        Status = StudySiteStatus.Inactive;
    }
}