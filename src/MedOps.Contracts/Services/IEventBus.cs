namespace MedOps.Contracts.Services;

public interface IEventBus
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : class;
}