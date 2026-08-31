namespace MedOps.Domain.Exceptions;

public class RequestNotFoundException : DomainException
{
    public Guid RequestId { get; }

    public RequestNotFoundException(Guid requestId) : base($"Request with ID '{requestId}' was not found.", "REQUEST_NOT_FOUND")
    {
        RequestId = requestId;
    }
}