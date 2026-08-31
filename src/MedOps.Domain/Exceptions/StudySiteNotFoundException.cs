namespace MedOps.Domain.Exceptions;

public class StudySiteNotFoundException : DomainException
{
    public Guid StudyId { get; }
    public Guid SiteId { get; }

    public StudySiteNotFoundException(Guid studyId, Guid siteId) : base($"StudySite linking Study '{studyId}' and Site '{siteId}' was not found.", "STUDY_SITE_NOT_FOUND")
    {
        StudyId = studyId;
        SiteId = siteId;
    }
}