namespace MedOps.Domain.Exceptions;

public class SiteNotFoundException : DomainException
{
    public Guid SiteId { get; }

    public SiteNotFoundException(Guid siteId) : base($"Site with ID '{siteId}' was not found.", "SITE_NOT_FOUND")
    {
        SiteId = siteId;
    }
}