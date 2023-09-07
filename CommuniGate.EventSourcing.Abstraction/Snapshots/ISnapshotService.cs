using CommuniGate.EventSourcing.Abstraction.Aggregates;

namespace CommuniGate.EventSourcing.Abstraction.Snapshots
{
    public interface ISnapshotService<TAggregate>
        where TAggregate : IAggregate
    {
        Task Store(TAggregate aggregate, CancellationToken cancellationToken);

        Task<TAggregate?> Restore(Guid aggregateId, CancellationToken cancellationToken);
    }
}
