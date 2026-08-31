using Microsoft.EntityFrameworkCore;
using MedOps.Domain.Entities;
using MedOps.Domain.Enums;
using MedOps.Infrastructure.Data;
using MedOps.Infrastructure.Repositories;

namespace MedOps.IntegrationTests;

public class RepositoryTests : IDisposable
{
    private readonly MedOpsDbContext _context;
    private readonly Repository<Study> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MedOpsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new MedOpsDbContext(options);
        _repository = new Repository<Study>(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistEntity()
    {
        var study = new Study("Test Study", "Description", Guid.NewGuid());

        await _repository.AddAsync(study);

        var result = await _repository.GetByIdAsync(study.Id);
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Study");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        await _repository.AddAsync(new Study("Study 1", "Desc 1", Guid.NewGuid()));
        await _repository.AddAsync(new Study("Study 2", "Desc 2", Guid.NewGuid()));

        var results = await _repository.GetAllAsync();
        results.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        var study = new Study("Original", "Description", Guid.NewGuid());
        await _repository.AddAsync(study);

        study.UpdateDetails("Updated Name", "Updated Description");
        await _repository.UpdateAsync(study);

        var result = await _repository.GetByIdAsync(study.Id);
        result!.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        var study = new Study("To Delete", "Description", Guid.NewGuid());
        await _repository.AddAsync(study);

        await _repository.DeleteAsync(study.Id);

        var result = await _repository.GetByIdAsync(study.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());
        result.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
