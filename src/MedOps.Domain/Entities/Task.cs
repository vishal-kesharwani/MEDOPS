namespace MedOps.Domain.Entities;

using MedOps.Domain.Enums;
using MedOps.Domain.Exceptions;

public class Task
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TaskStatus Status { get; private set; }
    public TaskPriority Priority { get; private set; }
    public Guid AssignedTo { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public Guid? StudyId { get; private set; }
    public ICollection<TaskAssignment> Assignments { get; private set; } = new List<TaskAssignment>();

    private Task() { }

    public Task(string title, string description, Guid assignedTo, Guid createdBy, Guid? studyId = null, DateOnly? dueDate = null)
    {
        Id = Guid.NewGuid();
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        AssignedTo = assignedTo;
        CreatedBy = createdBy;
        StudyId = studyId;
        DueDate = dueDate;
        Status = TaskStatus.ToDo;
        Priority = TaskPriority.Medium;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Start()
    {
        if (Status != TaskStatus.ToDo && Status != TaskStatus.Cancelled)
            throw new DomainException("Only ToDo or Cancelled tasks can be started.", "INVALID_TASK_TRANSITION");
        Status = TaskStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != TaskStatus.InProgress)
            throw new DomainException("Only InProgress tasks can be completed.", "INVALID_TASK_TRANSITION");
        Status = TaskStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = TaskStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPriority(TaskPriority priority)
    {
        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignTo(Guid userId)
    {
        AssignedTo = userId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string description)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UpdatedAt = DateTime.UtcNow;
    }
}