using MedOps.Domain.Entities;
using MedOps.Domain.Enums;
using MedOps.Domain.Exceptions;

namespace MedOps.UnitTests.Domain;

public class StudyTests
{
    [Fact]
    public void CreateStudy_ShouldInitializeWithCorrectDefaults()
    {
        var study = new Study("Test Study", "Description", Guid.NewGuid());

        study.Name.Should().Be("Test Study");
        study.Description.Should().Be("Description");
        study.Status.Should().Be(StudyStatus.Draft);
        study.Id.Should().NotBe(Guid.Empty);
        study.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void CreateStudy_ShouldThrowOnNullName()
    {
        Action act = () => new Study(null!, "Description", Guid.NewGuid());
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Activate_ShouldChangeStatus()
    {
        var study = new Study("Test", "Desc", Guid.NewGuid());
        study.Activate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(30)));
        study.Status.Should().Be(StudyStatus.Active);
    }

    [Fact]
    public void Complete_ShouldChangeStatus()
    {
        var study = new Study("Test", "Desc", Guid.NewGuid());
        study.Activate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(30)));
        study.Complete();
        study.Status.Should().Be(StudyStatus.Completed);
    }

    [Fact]
    public void Suspend_ShouldChangeStatus()
    {
        var study = new Study("Test", "Desc", Guid.NewGuid());
        study.Activate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(30)));
        study.Suspend();
        study.Status.Should().Be(StudyStatus.Suspended);
    }

    [Fact]
    public void Terminate_ShouldChangeStatus()
    {
        var study = new Study("Test", "Desc", Guid.NewGuid());
        study.Activate(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(30)));
        study.Terminate();
        study.Status.Should().Be(StudyStatus.Terminated);
    }

    [Fact]
    public void Complete_FromDraft_ShouldThrow()
    {
        var study = new Study("Test", "Desc", Guid.NewGuid());
        Action act = () => study.Complete();
        act.Should().Throw<DomainException>();
    }
}
