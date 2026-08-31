namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;

public interface IRequestService
{
    Task<List<RequestDto>> GetAllAsync();
    Task<RequestDto?> GetByIdAsync(Guid id);
    Task<RequestDto> CreateAsync(CreateRequestDto dto, Guid userId);
    Task ApproveAsync(Guid id, Guid approvedBy);
    Task RejectAsync(Guid id, Guid rejectedBy, string comment);
    Task CancelAsync(Guid id);
    Task DeleteAsync(Guid id);
}