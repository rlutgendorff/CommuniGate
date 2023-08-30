using CommuniGate.Events;

namespace CommuniGate.EventSourcing;

public interface IEventHandler<in TEntity, in TEvent>
    where TEvent : class, IEvent
{
    public void HandleAsync(TEntity entity, TEvent @event, CancellationToken cancellationToken = default);
}