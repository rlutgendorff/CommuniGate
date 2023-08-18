using CommuniGate.Commands;
using CommuniGate.Queries;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CommuniGate;

public class CommuniGator : ICommuniGator
{
    private readonly Container _container;

    
    public CommuniGator(Container container)
    {
        _container = container;
    }

    public Task<IResult<TResponse>> Execute<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        return Execute<IResult<TResponse>>(handlerType, query, cancellationToken);

        //var middlewares = _container.GetInstances<IPipelineMiddleware<IQuery<TResult>, TResult>>();

        //using (AsyncScopedLifestyle.BeginScope(_container))
        //{

        //    dynamic handler = _container.GetInstance(handlerType);
        //    return handler.HandleAsync((dynamic)query, cancellationToken);
        //}
    }

    public Task<IResult<TResponse>> Execute<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        return Execute<IResult<TResponse>>(handlerType, command, cancellationToken);

        //using (AsyncScopedLifestyle.BeginScope(_container))
        //{
        //    var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        //    dynamic handler = _container.GetInstance(handlerType);
        //    return handler.HandleAsync((dynamic)command, cancellationToken);
        //}
    }

    public Task<IResult> Execute(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        return Execute<IResult>(handlerType, command, cancellationToken);
    }

    private Task<TResponse> Execute<TResponse>(Type handlerType, object obj, CancellationToken cancellationToken = default)
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
        {
            dynamic handler = _container.GetInstance(handlerType);
            return handler.HandleAsync((dynamic)obj, cancellationToken);
        }
    }
}