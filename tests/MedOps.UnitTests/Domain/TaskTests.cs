using MedOps.Domain.Entities;
using MedOps.Domain.Enums;
using MedOps.Domain.Exceptions;

namespace MedOps.UnitTests.Domain;

public class TaskTests
{
    [Fact]
    public void CreateTask_ShouldInitializeWithCorrectDefaults()
    {
        var task = new MedOps.Domain.Entities.Task("Test Task", "Description", Guid.NewGuid(), Guid.NewGuid());

        task.Title.Should().Be("Test Task");
        task.Description.Should().Be("Description");
        task.Status.Should().Be(MedOps.Domain.Enums.TaskStatus.ToDo);
        task.Priority.Should().Be(TaskPriority.Medium);
        task.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Start_ShouldChangeStatus()
    {
        var task = new MedOps.Domain.Entities.Task("Test", "Desc", Guid.NewGuid(), Guid.NewGuid());
        task.Start();
        task.Status.Should().Be(MedOps.Domain.Enums.TaskStatus.InProgress);
    }

    [Fact]
    public void Complete_FromInProgress_ShouldChangeStatus()
    {
        var task = new MedOps.Domain.Entities.Task("Test", "Desc", Guid.NewGuid(), Guid.NewGuid());
        task.Start();
        task.Complete();
        task.Status.Should().Be(MedOps.Domain.Enums.TaskStatus.Completed);
    }

    [Fact]
    public void Complete_FromToDo_ShouldThrow()
    {
        var task = new MedOps.Domain.Entities.Task("Test", "Desc", Guid.NewGuid(), Guid.NewGuid());
        Action act = () => task.Complete();
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Cancel_ShouldChangeStatus()
    {
        var task = new MedOps.Domain.Entities.Task("Test", "Desc", Guid.NewGuid(), Guid.NewGuid());
        task.Cancel();
        task.Status.Should().Be(MedOps.Domain.Enums.TaskStatus.Cancelled);
    }

    [Fact]
    public void Start_FromCancelled_ShouldChangeStatus()
    {
        var task = new MedOps.Domain.Entities.Task("Test", "Desc", Guid.NewGuid(), Guid.NewGuid());
        task.Cancel();
        task.Start();
        task.Status.Should().Be(MedOps.Domain.Enums.TaskStatus.InProgress);
    }

    [Fact]
    public void SetPriority_ShouldUpdatePriority()
    {
        var task = new MedOps.Domain.Entities.Task("Test", "Desc", Guid.NewGuid(), Guid.NewGuid());
        task.SetPriority(TaskPriority.High);
        task.Priority.Should().Be(TaskPriority.High);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateTitleAndDescription()
    {
        var task = new MedOps.Domain.Entities.Task("Old Title", "Old Desc", Guid.NewGuid(), Guid.NewGuid());
        task.UpdateDetails("New Title", "New Desc");
        task.Title.Should().Be("New Title");
        task.Description.Should().Be("New Desc");
    }

    [Fact]
    public void AssignTo_ShouldUpdateAssignedTo()
    {
        var task = new MedOps.Domain.Entities.Task("Test", "Desc", Guid.NewGuid(), Guid.NewGuid());
        var newUserId = Guid.NewGuid();
        task.AssignTo(newUserId);
        task.AssignedTo.Should().Be(newUserId);
    }
}
