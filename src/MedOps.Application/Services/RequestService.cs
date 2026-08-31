namespace MedOps.Application.Services;

using MedOps.Domain.Enums;
using MedOps.Domain.Exceptions;
using MedOps.Domain.Interfaces;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Application.Validators;

public class RequestService : IRequestService
{
    private readonly IRepository<Request> _requestRepository;
    private readonly CreateRequestValidator _createValidator;

    public RequestService(IRepository<Request> requestRepository, CreateRequestValidator createValidator)
    {
        _requestRepository = requestRepository;
        _createValidator = createValidator;
    }

    public async Task<List<RequestDto>> GetAllAsync()
    {
        var requests = await _requestRepository.GetAllAsync();
        return requests.Select(r => new RequestDto
        {
            Id = r.Id, Title = r.Title, Description = r.Description, Status = r.Status,
            Priority = r.Priority, RequestedBy = r.RequestedBy, CreatedAt = r.CreatedAt, UpdatedAt = r.UpdatedAt,
            Approvals = r.Approvals.Select(a => new RequestApprovalDto
            {
                Id = a.Id, RequestId = a.RequestId, ApprovedBy = a.ApprovedBy,
                Status = a.Status, Comment = a.Comment, CommentedAt = a.CommentedAt
            }).ToList()
        }).ToList();
    }

    public async Task<RequestDto?> GetByIdAsync(Guid id)
    {
        var request = await _requestRepository.GetByIdAsync(id) ?? throw new RequestNotFoundException(id);
        return new RequestDto
        {
            Id = request.Id, Title = request.Title, Description = request.Description, Status = request.Status,
            Priority = request.Priority, RequestedBy = request.RequestedBy, CreatedAt = request.CreatedAt, UpdatedAt = request.UpdatedAt,
            Approvals = request.Approvals.Select(a => new RequestApprovalDto
            {
                Id = a.Id, RequestId = a.RequestId, ApprovedBy = a.ApprovedBy,
                Status = a.Status, Comment = a.Comment, CommentedAt = a.CommentedAt
            }).ToList()
        };
    }

    public async Task<RequestDto> CreateAsync(CreateRequestDto dto, Guid userId)
    {
        await _createValidator.ValidateAndThrowAsync(dto);
        var request = new Request(dto.Title, dto.Description, userId, dto.Priority, dto.StudyId);
        await _requestRepository.AddAsync(request);
        return new RequestDto
        {
            Id = request.Id, Title = request.Title, Description = request.Description, Status = request.Status,
            Priority = request.Priority, RequestedBy = request.RequestedBy, CreatedAt = request.CreatedAt, UpdatedAt = request.UpdatedAt
        };
    }

    public async Task ApproveAsync(Guid id, Guid approvedBy)
    {
        var request = await _requestRepository.GetByIdAsync(id) ?? throw new RequestNotFoundException(id);
        request.Approve(approvedBy);
        await _requestRepository.UpdateAsync(request);
    }

    public async Task RejectAsync(Guid id, Guid rejectedBy, string comment)
    {
        var request = await _requestRepository.GetByIdAsync(id) ?? throw new RequestNotFoundException(id);
        request.Reject(rejectedBy, comment);
        await _requestRepository.UpdateAsync(request);
    }

    public async Task CancelAsync(Guid id)
    {
        var request = await _requestRepository.GetByIdAsync(id) ?? throw new RequestNotFoundException(id);
        request.Cancel();
        await _requestRepository.UpdateAsync(request);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _requestRepository.DeleteAsync(id);
    }
}