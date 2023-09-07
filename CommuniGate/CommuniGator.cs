using CommuniGate.Commands;
using CommuniGate.Container.Abstraction;
using CommuniGate.Middlewares;
using CommuniGate.Queries;
using CommuniGate.Results;

namespace CommuniGate;

public class CommuniGator : ICommuniGator
{
    private readonly ICommuniGateContainer _container;


    public CommuniGator(ICommuniGateContainer container)
    {
        _container = container;
    }

    public Task<IResult<TResponse>> Execute<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));

        return Execute<IQuery<TResponse>, TResponse>(handlerType, query, cancellationToken);
    }

    public Task<IResult<TResponse>> Execute<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        return Execute<ICommand<TResponse>, TResponse>(handlerType, command, cancellationToken);
    }

    public Task<IResult> Execute(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        return Execute(handlerType, command, cancellationToken);
    }

   

    public Task Execute(Func<ICommuniGateContainer, Task> func)
    {
        using (_container.CreateScope())
        {
            return func(_container);
        }
    }

    public Task<IResult> Execute(Func<ICommuniGateContainer, Task<IResult>> func)
    {
        using (_container.CreateScope())
        {
            return func(_container);
        }
    }

    public Task<IResult<TResponse>> Execute<TResponse>(Func<ICommuniGateContainer, Task<IResult<TResponse>>> func)
    {
        using (_container.CreateScope())
        {
            return func(_container);
        }
    }

    internal Task<IResult<TResponse>> Execute<TCommunication, TResponse>(Type handlerType, TCommunication obj, CancellationToken cancellationToken = default)
    {
        using (_container.CreateScope())
        {
            Task<IResult<TResponse>> Handler() => (Task<IResult<TResponse>>)
                ((dynamic)_container.GetInstance(handlerType))
                .HandleAsync((dynamic)obj!, cancellationToken);

            var pipeline = _container.GetAllInstances<IPipelineMiddleware<TCommunication, TResponse>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)Handler,
                    (next, pipeline) => () => pipeline.Handle((dynamic)obj!, next, cancellationToken));

                var e = new ExceptionHandlingMiddleware<TCommunication, TResponse>();

                return e.Handle(obj, pipeline, cancellationToken);
        }
    }

    internal Task<IResult> Execute<TCommunication>(Type handlerType, TCommunication obj, CancellationToken cancellationToken = default)
    {
        using (_container.CreateScope())
        {
            Task<IResult> Handler() => (Task<IResult>)
                ((dynamic)_container.GetInstance(handlerType))
                .HandleAsync((dynamic)obj!, cancellationToken);

            var pipeline = _container.GetAllInstances<IPipelineMiddleware<TCommunication>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate)Handler,
                    (next, pipeline) => () => pipeline.Handle((dynamic)obj!, next, cancellationToken));

            var e = new ExceptionHandlingMiddleware<TCommunication>();

            return e.Handle(obj, pipeline, cancellationToken);
        }
    }
}