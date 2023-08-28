using CommuniGate.Commands;
using CommuniGate.Middlewares;
using CommuniGate.Queries;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using CommuniGate.Events;
using CommuniGate.Results;

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

        return ExecuteWithResponse<IQuery<TResponse>, TResponse>(handlerType, query, cancellationToken);
    }

    public Task<IResult<TResponse>> Execute<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        return ExecuteWithResponse<ICommand<TResponse>, TResponse>(handlerType, command, cancellationToken);
    }

    public Task<IResult> Execute(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        return ExecuteWithoutResponse<ICommand>(handlerType, command, cancellationToken);
    }

    public Task Publish(IEvent @event, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        return Publish(handlerType, @event, cancellationToken);
    }

    private Task Publish(Type handlerType, IEvent @event, CancellationToken cancellationToken = default)
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
        {
            Task Handler()
            {
                var handlers = (List<dynamic>)_container.GetAllInstances(handlerType).ToList();

                return Task
                    .WhenAll(handlers.Select(x => x.HandleAsync((dynamic)@event, cancellationToken))
                    .Cast<Task>()
                    .ToArray());
            }

            return _container
                .GetAllInstances<IEventPipelineMiddleware<IEvent>>()
                .Reverse()
                .Aggregate((EventHandlerDelegate)Handler,
                    (next, pipeline) => () => pipeline.Handle((dynamic)@event, next, cancellationToken))();
        }
    }

    private Task<IResult<TResponse>> ExecuteWithResponse<TCommunication, TResponse>(Type handlerType, TCommunication obj, CancellationToken cancellationToken = default)
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
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

    private Task<IResult> ExecuteWithoutResponse<TCommunication>(Type handlerType, TCommunication obj, CancellationToken cancellationToken = default)
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
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