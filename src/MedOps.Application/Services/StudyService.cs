namespace MedOps.Application.Services;

using MedOps.Domain.Enums;
using MedOps.Domain.Exceptions;
using MedOps.Domain.Interfaces;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Application.Validators;

public class StudyService : IStudyService
{
    private readonly IRepository<Study> _studyRepository;
    private readonly CreateStudyValidator _createValidator;
    private readonly UpdateStudyValidator _updateValidator;

    public StudyService(IRepository<Study> studyRepository, CreateStudyValidator createValidator, UpdateStudyValidator updateValidator)
    {
        _studyRepository = studyRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<List<StudyDto>> GetAllAsync()
    {
        var studies = await _studyRepository.GetAllAsync();
        return studies.Select(s => new StudyDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Status = s.Status,
            StartDate = s.StartDate,
            EndDate = s.EndDate,
            CreatedBy = s.CreatedBy,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();
    }

    public async Task<StudyDto?> GetByIdAsync(Guid id)
    {
        var study = await _studyRepository.GetByIdAsync(id) ?? throw new StudyNotFoundException(id);
        return new StudyDto
        {
            Id = study.Id,
            Name = study.Name,
            Description = study.Description,
            Status = study.Status,
            StartDate = study.StartDate,
            EndDate = study.EndDate,
            CreatedBy = study.CreatedBy,
            CreatedAt = study.CreatedAt,
            UpdatedAt = study.UpdatedAt
        };
    }

    public async Task<StudyDto> CreateAsync(CreateStudyDto dto, Guid userId)
    {
        await _createValidator.ValidateAndThrowAsync(dto);
        var study = new Study(dto.Name, dto.Description, userId);
        if (dto.StartDate.HasValue && dto.EndDate.HasValue)
            study.Activate(dto.StartDate.Value, dto.EndDate.Value);
        await _studyRepository.AddAsync(study);
        return new StudyDto
        {
            Id = study.Id, Name = study.Name, Description = study.Description,
            Status = study.Status, StartDate = study.StartDate, EndDate = study.EndDate,
            CreatedBy = study.CreatedBy, CreatedAt = study.CreatedAt, UpdatedAt = study.UpdatedAt
        };
    }

    public async Task<StudyDto> UpdateAsync(Guid id, UpdateStudyDto dto)
    {
        await _updateValidator.ValidateAndThrowAsync(dto);
        var study = await _studyRepository.GetByIdAsync(id) ?? throw new StudyNotFoundException(id);
        study.UpdateDetails(dto.Name, dto.Description);
        await _studyRepository.UpdateAsync(study);
        return new StudyDto
        {
            Id = study.Id, Name = study.Name, Description = study.Description,
            Status = study.Status, StartDate = study.StartDate, EndDate = study.EndDate,
            CreatedBy = study.CreatedBy, CreatedAt = study.CreatedAt, UpdatedAt = study.UpdatedAt
        };
    }

    public async Task ActivateAsync(Guid id, DateOnly startDate, DateOnly endDate)
    {
        var study = await _studyRepository.GetByIdAsync(id) ?? throw new StudyNotFoundException(id);
        study.Activate(startDate, endDate);
        await _studyRepository.UpdateAsync(study);
    }

    public async Task CompleteAsync(Guid id)
    {
        var study = await _studyRepository.GetByIdAsync(id) ?? throw new StudyNotFoundException(id);
        study.Complete();
        await _studyRepository.UpdateAsync(study);
    }

    public async Task SuspendAsync(Guid id)
    {
        var study = await _studyRepository.GetByIdAsync(id) ?? throw new StudyNotFoundException(id);
        study.Suspend();
        await _studyRepository.UpdateAsync(study);
    }

    public async Task TerminateAsync(Guid id)
    {
        var study = await _studyRepository.GetByIdAsync(id) ?? throw new StudyNotFoundException(id);
        study.Terminate();
        await _studyRepository.UpdateAsync(study);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _studyRepository.DeleteAsync(id);
    }
}