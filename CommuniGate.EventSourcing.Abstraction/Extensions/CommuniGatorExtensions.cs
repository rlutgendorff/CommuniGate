using CommuniGate.Events;
using CommuniGate.EventSourcing.Abstraction;
using CommuniGate.EventSourcing.Abstraction.Aggregates;
using CommuniGate.EventSourcing.Abstraction.Middlewares;
using CommuniGate.Middlewares;

namespace CommuniGate.EventSourcing.Extensions;

public static class CommuniGatorExtensions
{
    public static Task Execute<TAggregate, TEvent>(this ICommuniGator communiGator, TAggregate entity, TEvent @event,
        CancellationToken cancellationToken = default)
        where TAggregate : class, IAggregate
        where TEvent : class, IEvent
    {
        var handlerType = typeof(IEventHandler<,>).MakeGenericType(typeof(TAggregate), typeof(TEvent));

        return communiGator.Execute(container =>
        {
            Task Handler() => 
                ((dynamic)container.GetInstance(handlerType))
                .HandleAsync((dynamic)entity!, @event, cancellationToken);

            var pipeline = container.GetAllInstances<IEventPipelineMiddleware<TAggregate, TEvent>>()
                .Reverse()
                .Aggregate((EventHandlerDelegate)Handler,
                    (next, pipeline) => () =>
                        pipeline.Handle((dynamic)entity!, (dynamic)@event, next, cancellationToken));

            return pipeline();

            //var e = new ExceptionHandlingMiddleware<TCommunication, TResponse>();

            //return e.Handle(obj, pipeline, cancellationToken);
        });
    }

    public static Task Publish(this ICommuniGator communiGator, IEvent @event, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IEventNotificationHandler<>).MakeGenericType(@event.GetType());
        return communiGator.Execute(container =>
        {
            Task Handler()
            {
                var handlers = (List<dynamic>)container.GetAllInstances(handlerType).ToList();

                return Task
                    .WhenAll(handlers.Select(x => x.HandleAsync((dynamic)@event, cancellationToken))
                        .Cast<Task>()
                        .ToArray());
            }

            return container
                .GetAllInstances<IEventPipelineMiddleware<IEvent>>()
                .Reverse()
                .Aggregate((EventHandlerDelegate)Handler,
                    (next, pipeline) => () => pipeline.Handle((dynamic)@event, next, cancellationToken))();
        });
    }
}