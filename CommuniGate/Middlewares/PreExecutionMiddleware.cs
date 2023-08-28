using CommuniGate.Results;
using SimpleInjector;

namespace CommuniGate.Middlewares;

public class PreExecutionMiddleware<TRequest> : IPipelineMiddleware<TRequest>
{
    private readonly Container _container;

    public PreExecutionMiddleware(Container container)
    {
        _container = container;
    }

    public Task<IResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken)
    {
        var cType = typeof(IPreExecution<>).MakeGenericType(request!.GetType());
        var processors = _container.GetAllInstances(cType).Cast<dynamic>().ToList();

        processors.ForEach(x => x.Process((dynamic)request));

        var result = next.Invoke();

        return result;
    }
}

public class PreExecutionMiddleware<TRequest, TResponse> : IPipelineMiddleware<TRequest, TResponse>
{
    private readonly Container _container;

    public PreExecutionMiddleware(Container container)
    {
        _container = container;
    }

    public Task<IResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cType = typeof(IPreExecution<>).MakeGenericType(request!.GetType());
        var processors = _container.GetAllInstances(cType).Cast<dynamic>().ToList();

        processors.ForEach(x => x.Process((dynamic)request));

        var result = next.Invoke();

        return result;
    }
}

public class PreEventExecutionMiddleware<TEvent> : IEventPipelineMiddleware<TEvent>
{
    private readonly Container _container;

    public PreEventExecutionMiddleware(Container container)
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