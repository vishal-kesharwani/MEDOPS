namespace MedOps.Application.Services;

using MedOps.Domain.Interfaces;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Application.Validators;

public class DepartmentService : IDepartmentService
{
    private readonly IRepository<Department> _departmentRepository;
    private readonly CreateDepartmentValidator _createValidator;

    public DepartmentService(IRepository<Department> departmentRepository, CreateDepartmentValidator createValidator)
    {
        _departmentRepository = departmentRepository;
        _createValidator = createValidator;
    }

    public async Task<List<DepartmentDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(d => new DepartmentDto
        {
            Id = d.Id, Name = d.Name, Description = d.Description, CreatedAt = d.CreatedAt, UpdatedAt = d.UpdatedAt
        }).ToList();
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Department with ID '{id}' was not found.");
        return new DepartmentDto
        {
            Id = department.Id, Name = department.Name, Description = department.Description,
            CreatedAt = department.CreatedAt, UpdatedAt = department.UpdatedAt
        };
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);
        var department = new Department(dto.Name, dto.Description);
        await _departmentRepository.AddAsync(department);
        return new DepartmentDto
        {
            Id = department.Id, Name = department.Name, Description = department.Description,
            CreatedAt = department.CreatedAt, UpdatedAt = department.UpdatedAt
        };
    }

    public async Task<DepartmentDto> UpdateAsync(Guid id, UpdateDepartmentDto dto)
    {
        var department = await _departmentRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Department with ID '{id}' was not found.");
        department.UpdateDetails(dto.Name, dto.Description);
        await _departmentRepository.UpdateAsync(department);
        return new DepartmentDto
        {
            Id = department.Id, Name = department.Name, Description = department.Description,
            CreatedAt = department.CreatedAt, UpdatedAt = department.UpdatedAt
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        await _departmentRepository.DeleteAsync(id);
    }
}