namespace CommuniGate.EventSourcing.Abstraction.Projections;

public interface IProjectionRebuilder<TEntity>
{
    Task RebuildAsync(Func<EventWrapper, Task> action, CancellationToken cancellationToken);
}