using CommuniGate.Results;
using SimpleInjector;

namespace CommuniGate.Middlewares;

public class PreExecutionMiddleware<TRequest> : IPipelineMiddleware<TRequest>
{
    private readonly SimpleInjector.Container _container;

    public PreExecutionMiddleware(SimpleInjector.Container container)
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
    private readonly SimpleInjector.Container _container;

    public PreExecutionMiddleware(SimpleInjector.Container container)
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