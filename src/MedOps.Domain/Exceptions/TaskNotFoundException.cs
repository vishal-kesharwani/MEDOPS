namespace MedOps.Domain.Exceptions;

public class TaskNotFoundException : DomainException
{
    public Guid TaskId { get; }

    public TaskNotFoundException(Guid taskId) : base($"Task with ID '{taskId}' was not found.", "TASK_NOT_FOUND")
    {
        TaskId = taskId;
    }
}