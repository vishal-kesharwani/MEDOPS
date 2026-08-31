namespace MedOps.Api.Configurations;

using HotChocolate;
using MedOps.Application.DTOs;
using MedOps.Application.Interfaces;

public class StudyMutations
{
    private readonly IStudyService _studyService;

    public StudyMutations(IStudyService studyService)
    {
        _studyService = studyService;
    }

    public async Task<StudyDto> CreateStudyAsync(CreateStudyDto input)
    {
        return await _studyService.CreateAsync(input, Guid.NewGuid());
    }
}

public class SiteMutations
{
    private readonly ISiteService _siteService;

    public SiteMutations(ISiteService siteService)
    {
        _siteService = siteService;
    }

    public async Task<SiteDto> CreateSiteAsync(CreateSiteDto input)
    {
        return await _siteService.CreateAsync(input, Guid.NewGuid());
    }
}

public class TaskMutations
{
    private readonly ITaskService _taskService;

    public TaskMutations(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto input)
    {
        return await _taskService.CreateAsync(input);
    }
}