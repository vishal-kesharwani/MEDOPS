namespace MedOps.Api.Configurations;

using HotChocolate;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;

public class StudyQueries
{
    private readonly IStudyService _studyService;

    public StudyQueries(IStudyService studyService)
    {
        _studyService = studyService;
    }

    public async Task<StudyDto?> GetStudyByIdAsync(Guid id)
    {
        return await _studyService.GetByIdAsync(id);
    }

    public async Task<List<StudyDto>> GetAllStudiesAsync()
    {
        return await _studyService.GetAllAsync();
    }
}

public class SiteQueries
{
    private readonly ISiteService _siteService;

    public SiteQueries(ISiteService siteService)
    {
        _siteService = siteService;
    }

    public async Task<SiteDto?> GetSiteByIdAsync(Guid id)
    {
        return await _siteService.GetByIdAsync(id);
    }

    public async Task<List<SiteDto>> GetAllSitesAsync()
    {
        return await _siteService.GetAllAsync();
    }
}

public class TaskQueries
{
    private readonly ITaskService _taskService;

    public TaskQueries(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
    {
        return await _taskService.GetByIdAsync(id);
    }

    public async Task<List<TaskDto>> GetAllTasksAsync()
    {
        return await _taskService.GetAllAsync();
    }
}

public class RequestQueries
{
    private readonly IRequestService _requestService;

    public RequestQueries(IRequestService requestService)
    {
        _requestService = requestService;
    }

    public async Task<RequestDto?> GetRequestByIdAsync(Guid id)
    {
        return await _requestService.GetByIdAsync(id);
    }

    public async Task<List<RequestDto>> GetAllRequestsAsync()
    {
        return await _requestService.GetAllAsync();
    }
}