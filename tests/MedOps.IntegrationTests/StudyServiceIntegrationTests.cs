using Microsoft.EntityFrameworkCore;
using MedOps.Application.DTOs;
using MedOps.Application.Services;
using MedOps.Application.Validators;
using MedOps.Domain.Entities;
using MedOps.Infrastructure.Data;
using MedOps.Infrastructure.Repositories;

namespace MedOps.IntegrationTests;

public class StudyServiceIntegrationTests : IDisposable
{
    private readonly MedOpsDbContext _context;
    private readonly StudyService _service;

    public StudyServiceIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<MedOpsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MedOpsDbContext(options);
        var repository = new Repository<Study>(_context);
        _service = new StudyService(repository, new CreateStudyValidator(), new UpdateStudyValidator());
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistAndReturnStudy()
    {
        var dto = new CreateStudyDto { Name = "Integration Test Study", Description = "Test Description" };

        var result = await _service.CreateAsync(dto, Guid.NewGuid());

        result.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be("Integration Test Study");

        var fromDb = await _service.GetByIdAsync(result.Id);
        fromDb.Should().NotBeNull();
        fromDb!.Name.Should().Be("Integration Test Study");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCreatedStudies()
    {
        await _service.CreateAsync(new CreateStudyDto { Name = "Study 1", Description = "Desc 1" }, Guid.NewGuid());
        await _service.CreateAsync(new CreateStudyDto { Name = "Study 2", Description = "Desc 2" }, Guid.NewGuid());

        var results = await _service.GetAllAsync();
        results.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        var created = await _service.CreateAsync(new CreateStudyDto { Name = "Original", Description = "Desc" }, Guid.NewGuid());

        var updated = await _service.UpdateAsync(created.Id, new UpdateStudyDto { Name = "Updated", Description = "Updated Desc" });

        updated.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveStudy()
    {
        var created = await _service.CreateAsync(new CreateStudyDto { Name = "To Delete", Description = "Desc" }, Guid.NewGuid());

        await _service.DeleteAsync(created.Id);

        var all = await _service.GetAllAsync();
        all.Should().NotContain(s => s.Id == created.Id);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
