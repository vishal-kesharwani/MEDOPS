namespace MedOps.Contracts.Events;

public abstract record DomainEvent(Guid AggregateId, DateTime OccurredOn);

public record StudyCreatedEvent(Guid StudyId, string Name, Guid CreatedBy) : DomainEvent(StudyId, DateTime.UtcNow);
public record StudyActivatedEvent(Guid StudyId, DateOnly StartDate, DateOnly EndDate) : DomainEvent(StudyId, DateTime.UtcNow);
public record StudyCompletedEvent(Guid StudyId) : DomainEvent(StudyId, DateTime.UtcNow);
public record StudySuspendedEvent(Guid StudyId) : DomainEvent(StudyId, DateTime.UtcNow);
public record StudyTerminatedEvent(Guid StudyId) : DomainEvent(StudyId, DateTime.UtcNow);

public record SiteCreatedEvent(Guid SiteId, string Name, Guid CreatedBy) : DomainEvent(SiteId, DateTime.UtcNow);
public record SiteActivatedEvent(Guid SiteId) : DomainEvent(SiteId, DateTime.UtcNow);
public record SiteDeactivatedEvent(Guid SiteId) : DomainEvent(SiteId, DateTime.UtcNow);

public record TaskCreatedEvent(Guid TaskId, string Title, Guid AssignedTo, Guid CreatedBy) : DomainEvent(TaskId, DateTime.UtcNow);
public record TaskStartedEvent(Guid TaskId) : DomainEvent(TaskId, DateTime.UtcNow);
public record TaskCompletedEvent(Guid TaskId) : DomainEvent(TaskId, DateTime.UtcNow);
public record TaskCancelledEvent(Guid TaskId) : DomainEvent(TaskId, DateTime.UtcNow);

public record RequestCreatedEvent(Guid RequestId, string Title, Guid CreatedBy) : DomainEvent(RequestId, DateTime.UtcNow);
public record RequestApprovedEvent(Guid RequestId, Guid ApprovedBy) : DomainEvent(RequestId, DateTime.UtcNow);
public record RequestRejectedEvent(Guid RequestId, Guid RejectedBy, string Comment) : DomainEvent(RequestId, DateTime.UtcNow);
public record RequestCancelledEvent(Guid RequestId) : DomainEvent(RequestId, DateTime.UtcNow);

public record DepartmentCreatedEvent(Guid DepartmentId, string Name) : DomainEvent(DepartmentId, DateTime.UtcNow);
public record DepartmentUpdatedEvent(Guid DepartmentId, string Name) : DomainEvent(DepartmentId, DateTime.UtcNow);
public record DepartmentDeletedEvent(Guid DepartmentId) : DomainEvent(DepartmentId, DateTime.UtcNow);