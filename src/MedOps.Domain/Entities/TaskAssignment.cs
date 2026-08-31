namespace MedOps.Domain.Entities;

public class TaskAssignment
{
    public Guid Id { get; private set; }
    public Guid TaskId { get; private set; }
    public Guid AssignedTo { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime AssignedDate { get; private set; }
    public DateTime? CompletedDate { get; private set; }

    private TaskAssignment() { }

    public TaskAssignment(Guid taskId, Guid assignedTo)
    {
        Id = Guid.NewGuid();
        TaskId = taskId;
        AssignedTo = assignedTo;
        IsCompleted = false;
        AssignedDate = DateTime.UtcNow;
    }

    public void MarkCompleted()
    {
        IsCompleted = true;
        CompletedDate = DateTime.UtcNow;
    }
}