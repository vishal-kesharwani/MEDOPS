namespace MedOps.Domain.Entities;

using MedOps.Domain.Enums;

public class RequestApproval
{
    public Guid Id { get; private set; }
    public Guid RequestId { get; private set; }
    public Guid ApprovedBy { get; private set; }
    public ApprovalStatus Status { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    public DateTime CommentedAt { get; private set; }

    private RequestApproval() { }

    public RequestApproval(Guid requestId, Guid approvedBy, ApprovalStatus status, string comment = "")
    {
        Id = Guid.NewGuid();
        RequestId = requestId;
        ApprovedBy = approvedBy;
        Status = status;
        Comment = comment;
        CommentedAt = DateTime.UtcNow;
    }
}