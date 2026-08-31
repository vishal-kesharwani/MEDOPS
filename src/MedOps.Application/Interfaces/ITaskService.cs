namespace MedOps.Application.Interfaces;

using MedOps.Application.DTOs;

public interface ITaskService
{
    Task<List<TaskDto>> GetAllAsync();
    Task<TaskDto?> GetByIdAsync(Guid id);
    Task<TaskDto> CreateAsync(CreateTaskDto dto);
    Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto dto);
    Task StartAsync(Guid id);
    Task CompleteAsync(Guid id);
    Task CancelAsync(Guid id);
    Task DeleteAsync(Guid id);
}