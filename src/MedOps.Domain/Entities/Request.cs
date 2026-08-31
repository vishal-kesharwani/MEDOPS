namespace MedOps.Domain.Entities;

using MedOps.Domain.Enums;
using MedOps.Domain.Exceptions;

public class Request
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public RequestStatus Status { get; private set; }
    public string Priority { get; private set; } = "Medium";
    public Guid RequestedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Guid? StudyId { get; private set; }
    public ICollection<RequestApproval> Approvals { get; private set; } = new List<RequestApproval>();

    private Request() { }

    public Request(string title, string description, Guid requestedBy, string priority = "Medium", Guid? studyId = null)
    {
        Id = Guid.NewGuid();
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        RequestedBy = requestedBy;
        Priority = priority ?? throw new ArgumentNullException(nameof(priority));
        StudyId = studyId;
        Status = RequestStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve(Guid approvedBy)
    {
        if (Status != RequestStatus.Pending)
            throw new DomainException("Only pending requests can be approved.", "INVALID_REQUEST_TRANSITION");
        Status = RequestStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
        Approvals.Add(new RequestApproval(Id, approvedBy, ApprovalStatus.Approved));
    }

    public void Reject(Guid rejectedBy, string comment = "")
    {
        if (Status != RequestStatus.Pending)
            throw new DomainException("Only pending requests can be rejected.", "INVALID_REQUEST_TRANSITION");
        Status = RequestStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
        Approvals.Add(new RequestApproval(Id, rejectedBy, ApprovalStatus.Rejected, comment));
    }

    public void Cancel()
    {
        if (Status != RequestStatus.Pending)
            throw new DomainException("Only pending requests can be cancelled.", "INVALID_REQUEST_TRANSITION");
        Status = RequestStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}