using CommuniGate.EventSourcing.Abstraction.Middlewares;
using CommuniGate.Results;
using SimpleInjector;

namespace CommuniGate.Middlewares;


public class PostEventExecutionMiddleware<TEvent> : IEventPipelineMiddleware<TEvent>
{
    private readonly SimpleInjector.Container _container;

    public PostEventExecutionMiddleware(SimpleInjector.Container container)
    {
        _container = container;
    }

    public Task Handle(TEvent @event, EventHandlerDelegate next, CancellationToken cancellationToken)
    {
        var result = next.Invoke();

        var cType = typeof(IPostExecution<,>).MakeGenericType(@event!.GetType(), typeof(Task));
        var processors = _container.GetAllInstances(cType).Cast<dynamic>().ToList();

        processors.ForEach(x => x.Process((dynamic)@event, (dynamic)result));

        return result;
    }
}