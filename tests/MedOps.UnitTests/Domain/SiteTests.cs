using MedOps.Domain.Entities;
using MedOps.Domain.Enums;
using MedOps.Domain.Exceptions;

namespace MedOps.UnitTests.Domain;

public class SiteTests
{
    [Fact]
    public void CreateSite_ShouldInitializeWithCorrectDefaults()
    {
        var site = new Site("Test Site", "Description",
            new MedOps.Domain.ValueObjects.Address("123 Main St", "City", "State", "Country", "12345"),
            new MedOps.Domain.ValueObjects.ContactInfo("test@example.com", "555-1234"),
            Guid.NewGuid());

        site.Name.Should().Be("Test Site");
        site.Status.Should().Be(SiteStatus.Active);
        site.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Deactivate_ShouldChangeStatus()
    {
        var site = new Site("Test", "Desc",
            new MedOps.Domain.ValueObjects.Address("123 Main St", "City", "State", "Country", "12345"),
            new MedOps.Domain.ValueObjects.ContactInfo("test@example.com", "555-1234"),
            Guid.NewGuid());

        site.Deactivate();
        site.Status.Should().Be(SiteStatus.Inactive);
    }

    [Fact]
    public void Activate_ShouldChangeStatus()
    {
        var site = new Site("Test", "Desc",
            new MedOps.Domain.ValueObjects.Address("123 Main St", "City", "State", "Country", "12345"),
            new MedOps.Domain.ValueObjects.ContactInfo("test@example.com", "555-1234"),
            Guid.NewGuid());

        site.Deactivate();
        site.Activate();
        site.Status.Should().Be(SiteStatus.Active);
    }

    [Fact]
    public void Deactivate_FromInactive_ShouldThrow()
    {
        var site = new Site("Test", "Desc",
            new MedOps.Domain.ValueObjects.Address("123 Main St", "City", "State", "Country", "12345"),
            new MedOps.Domain.ValueObjects.ContactInfo("test@example.com", "555-1234"),
            Guid.NewGuid());

        site.Deactivate();
        Action act = () => site.Deactivate();
        act.Should().Throw<DomainException>();
    }
}
