namespace MedOps.Application.DTOs;

using MedOps.Domain.Enums;

public class RequestDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RequestStatus Status { get; set; }
    public string Priority { get; set; } = "Medium";
    public Guid RequestedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<RequestApprovalDto> Approvals { get; set; } = new List<RequestApprovalDto>();
}

public class RequestApprovalDto
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid ApprovedBy { get; set; }
    public ApprovalStatus Status { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CommentedAt { get; set; }
}

public class CreateRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public Guid? StudyId { get; set; }
}

public class UpdateRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}