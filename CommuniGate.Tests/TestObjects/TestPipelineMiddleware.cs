using CommuniGate.Bases;
using CommuniGate.Events;
using CommuniGate.Middlewares;
using CommuniGate.Queries;
using CommuniGate.Results;

namespace CommuniGate.Tests.TestObjects;

public class TestPipelineMiddleware : IPipelineMiddleware<ICommunication, string>
{
    public Task<IResult<string>> Handle(ICommunication communication, RequestHandlerDelegate<string> next, CancellationToken cancellationToken)
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
    public Task Handle(IEvent @event, EventHandlerDelegate next, CancellationToken cancellationToken)
    {
        return next.Invoke();
    }
}