using CommuniGate.Events;

namespace CommuniGate.EventSourcing.Extensions;

public static class CommuniGatorExtensions
{
    public static Task Execute<TEntity, TEvent>(this ICommuniGator communiGator, TEntity entity, TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var handlerType = typeof(IEventHandler<,>).MakeGenericType(typeof(TEntity), typeof(TEvent));

        return communiGator.Execute(container =>
        {
            Task Handler() => 
                ((dynamic)container.GetInstance(handlerType))
                .HandleAsync((dynamic)entity!, @event, cancellationToken);

            var pipeline = container.GetAllInstances<IEventPipelineMiddleware<TEntity, TEvent>>()
                .Reverse()
                .Aggregate((EventHandlerDelegate)Handler,
                    (next, pipeline) => () =>
                        pipeline.Handle((dynamic)entity!, (dynamic)@event, next, cancellationToken));

            return pipeline();

            //var e = new ExceptionHandlingMiddleware<TCommunication, TResponse>();

            //return e.Handle(obj, pipeline, cancellationToken);
        });
    }
}