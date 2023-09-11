using CommuniGate.EventSourcing.Abstraction.Aggregates;
using CommuniGate.EventSourcing.Abstraction.Snapshots;
using Marten;

namespace CommuniGate.EventSourcing.MartenDb;

public class MartenDbSnapshotService<TAggregate> : ISnapshotService<TAggregate>
    where TAggregate : IAggregate
{
    private readonly IDocumentSession _session;

    public MartenDbSnapshotService(IDocumentStore store)
    {
        _session = store.LightweightSession();
    }

    public Task Store(TAggregate aggregate, CancellationToken cancellationToken)
    {
        _session.Store(aggregate);
        return _session.SaveChangesAsync(cancellationToken);
    }

    public Task<TAggregate?> Restore(Guid aggregateId, CancellationToken cancellationToken)
    {
        return _session.LoadAsync<TAggregate>(aggregateId, cancellationToken);
    }
}