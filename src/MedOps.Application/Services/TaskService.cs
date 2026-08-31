namespace MedOps.Application.Services;

using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;
using MedOps.Application.Validators;
using MedOps.Domain.Exceptions;
using MedOps.Domain.Interfaces;

public class TaskService : ITaskService
{
    private readonly IRepository<MedOps.Domain.Entities.Task> _taskRepository;
    private readonly CreateTaskValidator _createValidator;
    private readonly UpdateTaskValidator _updateValidator;

    public TaskService(IRepository<MedOps.Domain.Entities.Task> taskRepository, CreateTaskValidator createValidator, UpdateTaskValidator updateValidator)
    {
        _taskRepository = taskRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<List<TaskDto>> GetAllAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        var result = new List<TaskDto>();
        foreach (var t in tasks)
        {
            result.Add(new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString(),
                AssignedTo = t.AssignedTo,
                CreatedBy = t.CreatedBy,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                DueDate = t.DueDate
            });
        }
        return result;
    }

    public async Task<TaskDto?> GetByIdAsync(Guid id)
    {
        var t = await _taskRepository.GetByIdAsync(id) ?? throw new TaskNotFoundException(id);
        return new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            AssignedTo = t.AssignedTo,
            CreatedBy = t.CreatedBy,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            DueDate = t.DueDate
        };
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);
        var priority = MedOps.Domain.Enums.TaskPriority.Medium;
        Enum.TryParse<MedOps.Domain.Enums.TaskPriority>(dto.Priority, true, out var p);
        if (Enum.IsDefined(typeof(MedOps.Domain.Enums.TaskPriority), p))
            priority = p;
        var task = new MedOps.Domain.Entities.Task(dto.Title, dto.Description, dto.AssignedTo, Guid.NewGuid(), dto.StudyId, dto.DueDate);
        task.SetPriority(priority);
        await _taskRepository.AddAsync(task);
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            AssignedTo = task.AssignedTo,
            CreatedBy = task.CreatedBy,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            DueDate = task.DueDate
        };
    }

    public async Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto dto)
    {
        await _updateValidator.ValidateAndThrowAsync(dto);
        var t = await _taskRepository.GetByIdAsync(id) ?? throw new TaskNotFoundException(id);
        if (!string.IsNullOrEmpty(dto.Title)) t.UpdateDetails(dto.Title, t.Description);
        if (!string.IsNullOrEmpty(dto.Description)) t.UpdateDetails(t.Title, dto.Description);
        var priority = MedOps.Domain.Enums.TaskPriority.Medium;
        MedOps.Domain.Enums.TaskPriority parsed;
        if (Enum.TryParse<MedOps.Domain.Enums.TaskPriority>(dto.Priority, true, out parsed) && Enum.IsDefined(typeof(MedOps.Domain.Enums.TaskPriority), parsed))
            priority = parsed;
        t.SetPriority(priority);
        await _taskRepository.UpdateAsync(t);
        return new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            AssignedTo = t.AssignedTo,
            CreatedBy = t.CreatedBy,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            DueDate = t.DueDate
        };
    }

    public async Task StartAsync(Guid id)
    {
        var t = await _taskRepository.GetByIdAsync(id) ?? throw new TaskNotFoundException(id);
        t.Start();
        await _taskRepository.UpdateAsync(t);
    }

    public async Task CompleteAsync(Guid id)
    {
        var t = await _taskRepository.GetByIdAsync(id) ?? throw new TaskNotFoundException(id);
        t.Complete();
        await _taskRepository.UpdateAsync(t);
    }

    public async Task CancelAsync(Guid id)
    {
        var t = await _taskRepository.GetByIdAsync(id) ?? throw new TaskNotFoundException(id);
        t.Cancel();
        await _taskRepository.UpdateAsync(t);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _taskRepository.DeleteAsync(id);
    }
}