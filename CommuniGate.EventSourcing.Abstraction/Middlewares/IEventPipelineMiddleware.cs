namespace CommuniGate.EventSourcing.Abstraction.Middlewares;

public delegate Task EventHandlerDelegate();

public interface IEventPipelineMiddleware<in TEvent>
{
    Task Handle(TEvent @event, EventHandlerDelegate next, CancellationToken cancellationToken);
}

public interface IEventPipelineMiddleware<in TEntity, in TEvent>
{
    Task Handle(TEntity entity, TEvent @event, EventHandlerDelegate next, CancellationToken cancellationToken);
}