namespace CommuniGate.Middlewares;

//public delegate Task<IResult<TResponse>> RequestHandlerDelegate<TResponse>();
//public delegate Task<IResult> RequestHandlerDelegate();


//public interface IPipelineMiddleware<in TRequest>
//    where TRequest : class, ICommunication
//{
//    Task<IResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken);
//}

//public interface IPipelineMiddleware<in TRequest, TResponse>
//    where TRequest : class, ICommunication<TResponse>
//{
//    Task<IResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
//}

public delegate Task<IResult<TResponse>> RequestHandlerDelegate<TResponse>();
public delegate Task<IResult> RequestHandlerDelegate();

public interface IPipelineMiddleware<in TRequest, TResponse>
{
    Task<IResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

public interface IPipelineMiddleware<in TRequest>
{

    Task<IResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken);
}