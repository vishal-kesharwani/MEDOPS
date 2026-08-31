using Moq;
using MedOps.Application.DTOs;
using MedOps.Application.Services;
using MedOps.Application.Validators;
using MedOps.Domain.Entities;
using MedOps.Domain.Enums;
using MedOps.Domain.Exceptions;
using MedOps.Domain.Interfaces;

namespace MedOps.UnitTests.Application;

public class StudyServiceTests
{
    private readonly Mock<IRepository<Study>> _repositoryMock = new();
    private readonly StudyService _service;

    public StudyServiceTests()
    {
        _service = new StudyService(
            _repositoryMock.Object,
            new CreateStudyValidator(),
            new UpdateStudyValidator());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnStudy_WhenExists()
    {
        var study = new Study("Test", "Desc", Guid.NewGuid());
        _repositoryMock.Setup(r => r.GetByIdAsync(study.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(study);

        var result = await _service.GetByIdAsync(study.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Test");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Study?)null);

        Func<Task> act = () => _service.GetByIdAsync(Guid.NewGuid());
        await act.Should().ThrowAsync<StudyNotFoundException>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStudies()
    {
        var studies = new List<Study>
        {
            new("Study 1", "Desc 1", Guid.NewGuid()),
            new("Study 2", "Desc 2", Guid.NewGuid())
        };
        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(studies);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateStudy_WhenValid()
    {
        var dto = new CreateStudyDto { Name = "New Study", Description = "Description" };

        var result = await _service.CreateAsync(dto, Guid.NewGuid());

        result.Name.Should().Be("New Study");
        result.Status.Should().Be(StudyStatus.Draft);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Study>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldActivate_WhenDatesProvided()
    {
        var dto = new CreateStudyDto
        {
            Name = "New Study",
            Description = "Description",
            StartDate = DateOnly.FromDateTime(DateTime.Today),
            EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30))
        };

        var result = await _service.CreateAsync(dto, Guid.NewGuid());

        result.Status.Should().Be(StudyStatus.Active);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStudy_WhenValid()
    {
        var study = new Study("Old Name", "Old Desc", Guid.NewGuid());
        _repositoryMock.Setup(r => r.GetByIdAsync(study.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(study);

        var dto = new UpdateStudyDto { Name = "New Name", Description = "New Desc" };
        var result = await _service.UpdateAsync(study.Id, dto);

        result.Name.Should().Be("New Name");
        result.Description.Should().Be("New Desc");
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepository()
    {
        var id = Guid.NewGuid();
        await _service.DeleteAsync(id);
        _repositoryMock.Verify(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
