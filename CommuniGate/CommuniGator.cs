using CommuniGate.Commands;
using CommuniGate.Middlewares;
using CommuniGate.Queries;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Threading;
using CommuniGate.Events;

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

    public Task Execute(IEvent @event, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        return Publish<IEvent>(handlerType, @event, cancellationToken);
    }

    private Task Publish<TEvent>(Type handlerType, TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
        {
            Task Handler() => Task.WhenAll(
                _container.GetAllInstances<IEventHandler<TEvent>>().ToList().Select(x=> x.HandleAsync(@event, cancellationToken)).ToArray());

            return _container.GetAllInstances<IEventPipelineMiddleware<TEvent>>()
                .Reverse()
                .Aggregate((EventHandlerDelegate)Handler,
                    (next, pipeline) => () => pipeline.Handle(@event, next, cancellationToken))();
        }
    }

    private Task<IResult<TResponse>> ExecuteWithResponse<TCommunication, TResponse>(Type handlerType, object obj, CancellationToken cancellationToken = default)
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
        {
            Task<IResult<TResponse>> Handler() => (Task<IResult<TResponse>>)
                ((dynamic)_container.GetInstance(handlerType))
                .HandleAsync((dynamic)obj, cancellationToken);

            return _container.GetAllInstances<IPipelineMiddleware<TCommunication, TResponse>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)Handler,
                    (next, pipeline) => () => pipeline.Handle((dynamic)obj, next, cancellationToken))();
        }
    }
    
    private Task<IResult> ExecuteWithoutResponse<TCommunication>(Type handlerType, object obj, CancellationToken cancellationToken = default)
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
        {
            Task<IResult> Handler() => (Task<IResult>)
                ((dynamic)_container.GetInstance(handlerType))
                .HandleAsync((dynamic)obj, cancellationToken);

            return _container.GetAllInstances<IPipelineMiddleware<TCommunication>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate)Handler,
                    (next, pipeline) => () => pipeline.Handle((dynamic)obj, next, cancellationToken))();
        }
    }

    
}