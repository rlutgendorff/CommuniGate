using CommuniGate.Results;

namespace CommuniGate.Middlewares;

public class ExceptionHandlingMiddleware<TRequest> : IPipelineMiddleware<TRequest>
{
    public async Task<IResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken)
    {
        try
        {
            return await next.Invoke();
        }
        catch (Exception ex)
        {
            return new Result(ex);
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