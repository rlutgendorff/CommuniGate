using CommuniGate.Results;

namespace CommuniGate.Middlewares;

public class ExceptionHandlingMiddleware<TRequest> : IPipelineMiddleware<TRequest>
{
    public Task<IResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken)
    {
        try
        {
            return next.Invoke();
        }
        catch (Exception ex)
        {
            return Task.FromResult<IResult>(new Result(ex));
        }
    }
}

public class ExceptionHandlingMiddleware<TRequest, TResponse> : IPipelineMiddleware<TRequest, TResponse>
{
    public async Task<IResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next.Invoke();
        }
        catch (Exception ex)
        {
            return new Result<TResponse>(ex);
        }
    }
}