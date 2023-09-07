using CommuniGate.EventSourcing.Abstraction.Middlewares;


namespace CommuniGate.Middlewares;


public class PreEventExecutionMiddleware<TEvent> : IEventPipelineMiddleware<TEvent>
{
    private readonly SimpleInjector.Container _container;

    public PreEventExecutionMiddleware(SimpleInjector.Container container)
    {
        _container = container;
    }

    public Task Handle(TEvent @event, EventHandlerDelegate next, CancellationToken cancellationToken)
    {
        var cType = typeof(IPreExecution<>).MakeGenericType(@event!.GetType());
        var processors = _container.GetAllInstances(cType).Cast<dynamic>().ToList();

        processors.ForEach(x => x.Process((dynamic)@event));

        var result = next.Invoke();

        return result;
    }
}