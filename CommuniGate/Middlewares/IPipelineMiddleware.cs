using CommuniGate.Results;

namespace CommuniGate.Middlewares;

public delegate Task<IResult<TResponse>> RequestHandlerDelegate<TResponse>();
public delegate Task<IResult> RequestHandlerDelegate();
public delegate Task EventHandlerDelegate();

public interface IPipelineMiddleware<in TRequest, TResponse>
{
    Task<IResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

public interface IPipelineMiddleware<in TRequest>
{
    Task<IResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken);
}

public interface IEventPipelineMiddleware<in TEvent>
{
    Task Handle(TEvent @event, EventHandlerDelegate next, CancellationToken cancellationToken);
}