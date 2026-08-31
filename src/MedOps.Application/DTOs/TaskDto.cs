namespace MedOps.Application.DTOs;

using MedOps.Domain.Enums;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public Guid AssignedTo { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateOnly? DueDate { get; set; }
}

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AssignedTo { get; set; }
    public string Priority { get; set; } = "Medium";
    public DateOnly? DueDate { get; set; }
    public Guid? StudyId { get; set; }
}

public class UpdateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public DateOnly? DueDate { get; set; }
}

public class TaskAssignmentDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid AssignedTo { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
}