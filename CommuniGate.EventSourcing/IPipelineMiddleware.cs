using CommuniGate.Results;

namespace CommuniGate.EventSourcing;

public delegate Task EventHandlerDelegate();

public interface IEventPipelineMiddleware<in TEntity, in TEvent>
{
    Task Handle(TEntity entity, TEvent @event, EventHandlerDelegate next, CancellationToken cancellationToken);
}