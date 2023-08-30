using CommuniGate.Events;
using CommuniGate.Results;

namespace CommuniGate.EventSourcing.Extensions;

public static class CommuniGatorExtensions
{
    public static void Execute<TEntity, TEvent>(this CommuniGator communiGator, TEntity entity, TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var handlerType = typeof(IEventHandler<,>).MakeGenericType(typeof(TEntity), typeof(TEvent));

        communiGator.Execute(container =>
        {
            Task<IResult> Handler() => (Task<IResult>)
                ((dynamic)container.GetInstance(handlerType))
                .HandleAsync((dynamic)entity!, @event, cancellationToken);

            var pipeline = container.GetAllInstances<IEventPipelineMiddleware<TEntity, TEvent>>()
                .Reverse()
                .Aggregate((EventHandlerDelegate)Handler,
                    (next, pipeline) => () =>
                        pipeline.Handle((dynamic)entity!, (dynamic)@event, next, cancellationToken));

            return (Task)pipeline();

            //var e = new ExceptionHandlingMiddleware<TCommunication, TResponse>();

            //return e.Handle(obj, pipeline, cancellationToken);
        });
    }
}