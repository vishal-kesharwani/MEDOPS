namespace MedOps.Domain.Exceptions;

public class StudyNotFoundException : DomainException
{
    public Guid StudyId { get; }

    public StudyNotFoundException(Guid studyId) : base($"Study with ID '{studyId}' was not found.", "STUDY_NOT_FOUND")
    {
        StudyId = studyId;
    }
}