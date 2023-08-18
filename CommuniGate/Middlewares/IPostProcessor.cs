namespace CommuniGate.Middlewares;

public interface IPostProcessor<in TRequest, TResponse>
    where TRequest : class, ICommunication
{
    Task Process(TRequest request, Task<IResult<TResponse>> response, CancellationToken cancellation = default);
}

public interface IPostProcessor<in TRequest>
    where TRequest : class, ICommunication
{
    Task Process(TRequest request, Task<IResult> response, CancellationToken cancellation = default);
}