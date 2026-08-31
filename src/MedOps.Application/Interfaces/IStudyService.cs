namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;

public interface IStudyService
{
    Task<List<StudyDto>> GetAllAsync();
    Task<StudyDto?> GetByIdAsync(Guid id);
    Task<StudyDto> CreateAsync(CreateStudyDto dto, Guid userId);
    Task<StudyDto> UpdateAsync(Guid id, UpdateStudyDto dto);
    Task ActivateAsync(Guid id, DateOnly startDate, DateOnly endDate);
    Task CompleteAsync(Guid id);
    Task SuspendAsync(Guid id);
    Task TerminateAsync(Guid id);
    Task DeleteAsync(Guid id);
}