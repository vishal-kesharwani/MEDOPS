namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(Guid id);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
    Task<DepartmentDto> UpdateAsync(Guid id, UpdateDepartmentDto dto);
    Task DeleteAsync(Guid id);
}