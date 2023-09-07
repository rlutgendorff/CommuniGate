using CommuniGate.Bases;
using CommuniGate.Middlewares;
using CommuniGate.Results;
using CommuniGate.TestHelpers.TestObjects.Handlers;

namespace CommuniGate.TestHelpers.TestObjects.Pipelines;

public class TestPipelineMiddleware : IPipelineMiddleware<WithResultCommand, int>
{
    public Task<IResult<int>> Handle(WithResultCommand request, RequestHandlerDelegate<int> next, CancellationToken cancellationToken)
    {
        return next.Invoke();
    }
}

public class TestGenericPipelineMiddleware<TRequest, TResponse> : IPipelineMiddleware<TRequest, TResponse>
{
    public Task<IResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        return next.Invoke();
    }
}

public class TestPipelineMiddlewareWithoutResponse : IPipelineMiddleware<ICommunication>
{
    public Task<IResult> Handle(ICommunication request, RequestHandlerDelegate next, CancellationToken cancellationToken)
    {
        return next.Invoke();
    }
}

public class TestEventPipelineMiddleware : IEventPipelineMiddleware<IEvent>
{
    public static event EventHandler? OnHandling;

    public Task Handle(IEvent @event, EventHandlerDelegate next, CancellationToken cancellationToken)
    {
        OnHandling?.Invoke(this, EventArgs.Empty);
        return next.Invoke();
    }
}