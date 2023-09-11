using CommuniGate.Events;
using CommuniGate.EventSourcing.Abstraction;
using CommuniGate.EventSourcing.Abstraction.Stores;
using Marten;

namespace CommuniGate.EventSourcing.MartenDb;

public class MartenDbEventStore : IEventStore
{
    private readonly IDocumentSession _session;

    public MartenDbEventStore(IDocumentStore store)
    {
        _session = store.LightweightSession();
    }

    public async Task<IEnumerable<EventWrapper>> ReadEventsAsync(Guid id, CancellationToken cancellationToken)
    {
        var events = await _session.Events.FetchStreamAsync(id, token: cancellationToken);

        return events.Select(CreateWrapper).ToList();
    }

    public Task AppendEventAsync(EventWrapper @event, CancellationToken cancellationToken)
    {

        if (@event.AggregateVersion > 0)
        {
            _session.Events.Append(@event.AggregateId, @event.AggregateVersion + 1, @event.Event);
        }
        else
        {
            _session.Events.StartStream(@event.AggregateId, @event.Event);
        }

        return _session.SaveChangesAsync(cancellationToken);
    }

    public Task<Queue<EventWrapper>> ReadAllStreams(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var events = _session.Events.QueryAllRawEvents();
            var result = new Queue<EventWrapper>();

            foreach (var @event in events)
            {
                if (@event.EventTypeName is "tombstone") continue;

                var wrapper = CreateWrapper(@event);

                result.Enqueue(wrapper);
            }

            return result;
        }, cancellationToken);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.Run(()=>_session.Delete(id), cancellationToken);
    }

    private static EventWrapper CreateWrapper(Marten.Events.IEvent @event)
    {
        var wrapper = new EventWrapper
        {
            AggregateId = @event.StreamId,
            AggregateVersion = @event.Version,
            Event = (IEvent)@event.Data,
            EventId = @event.Id,
            Metadata = new EventMetadata
            {
                Id = @event.StreamId,
                TypeName = @event.EventType.AssemblyQualifiedName,
                Metadata = new Dictionary<string, string>()
            }
        };

        return wrapper;
    }
}