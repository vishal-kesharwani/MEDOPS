namespace MedOps.Domain.Entities;

using MedOps.Domain.Enums;

public class StudyStaff
{
    public Guid Id { get; private set; }
    public Guid StudyId { get; private set; }
    public Guid UserId { get; private set; }
    public StaffRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime AssignedAt { get; private set; }

    private StudyStaff() { }

    public StudyStaff(Guid studyId, Guid userId, StaffRole role)
    {
        Id = Guid.NewGuid();
        StudyId = studyId;
        UserId = userId;
        Role = role;
        IsActive = true;
        AssignedAt = DateTime.UtcNow;
    }

    public void Remove()
    {
        IsActive = false;
    }
}