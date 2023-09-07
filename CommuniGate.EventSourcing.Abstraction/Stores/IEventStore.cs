namespace CommuniGate.EventSourcing.Abstraction.Stores;

public interface IEventStore
{
    Task<IEnumerable<EventWrapper>> ReadEventsAsync(Guid id, CancellationToken cancellationToken);

    Task AppendEventAsync(EventWrapper @event, CancellationToken cancellationToken);

    Task<Queue<EventWrapper>> ReadAllStreams(CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}