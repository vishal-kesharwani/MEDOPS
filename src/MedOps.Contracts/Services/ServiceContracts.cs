namespace MedOps.Contracts.Services;

public interface IStudyContract
{
    Task<StudyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<StudyDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface ISiteContract
{
    Task<SiteDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<SiteDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface ITaskContract
{
    Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<TaskDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface IRequestContract
{
    Task<RequestDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<RequestDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface IDepartmentContract
{
    Task<DepartmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

public record StudyDto(Guid Id, string Name, string Status);
public record SiteDto(Guid Id, string Name, string Status);
public record TaskDto(Guid Id, string Title, string Status, string Priority);
public record RequestDto(Guid Id, string Title, string Status);
public record DepartmentDto(Guid Id, string Name);