namespace MedOps.Domain.Entities;

using MedOps.Domain.Enums;
using MedOps.Domain.ValueObjects;
using MedOps.Domain.Exceptions;

public class Study
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public StudyStatus Status { get; private set; }
    public DateOnly? StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ICollection<StudySite> StudySites { get; private set; } = new List<StudySite>();
    public ICollection<StudyStaff> StudyStaff { get; private set; } = new List<StudyStaff>();
    public ICollection<Task> Tasks { get; private set; } = new List<Task>();
    public ICollection<Request> Requests { get; private set; } = new List<Request>();

    private Study() { }

    public Study(string name, string description, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        CreatedBy = createdBy;
        Status = StudyStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate(DateOnly startDate, DateOnly endDate)
    {
        if (Status != StudyStatus.Draft)
            throw new DomainException("Only draft studies can be activated.", "INVALID_STUDY_TRANSITION");
        Status = StudyStatus.Active;
        StartDate = startDate;
        EndDate = endDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != StudyStatus.Active)
            throw new DomainException("Only active studies can be completed.", "INVALID_STUDY_TRANSITION");
        Status = StudyStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        if (Status != StudyStatus.Active)
            throw new DomainException("Only active studies can be suspended.", "INVALID_STUDY_TRANSITION");
        Status = StudyStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Terminate()
    {
        if (Status != StudyStatus.Active && Status != StudyStatus.Suspended)
            throw new DomainException("Only active or suspended studies can be terminated.", "INVALID_STUDY_TRANSITION");
        Status = StudyStatus.Terminated;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UpdatedAt = DateTime.UtcNow;
    }
}