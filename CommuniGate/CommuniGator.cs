using CommuniGate.Commands;
using CommuniGate.Middlewares;
using CommuniGate.Queries;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Threading;
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

    public Task<IResult<TResponse>> ExecuteQuery<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default) 
        where TQuery : class, IQuery<TResponse>
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
        {
            var handler = _container.GetInstance<IQueryHandler<TQuery, TResponse>>();
            return handler.HandleAsync(query, cancellationToken);
        }
    }

    public Task<IResult<TResponse>> ExecuteCommand<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand<TResponse>
    {
        var pipelineBehaviors = _container.GetAllInstances<IPipelineMiddleware<TCommand, TResponse>>();
        var reversedBehaviors = new List<IPipelineMiddleware<TCommand, TResponse>>(pipelineBehaviors);
        reversedBehaviors.Reverse();

        RequestHandlerDelegate<TResponse> currentDelegate = () => Handle<TCommand, TResponse>(command, cancellationToken);
        foreach (var pipeline in reversedBehaviors)
        {
            var nextDelegate = currentDelegate;
            currentDelegate = () => pipeline.Handle(command, nextDelegate, cancellationToken);
        }

        return currentDelegate();
    }

    private Task<IResult<TResponse>> Handle<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken) 
        where TCommand : class, ICommand<TResponse>
    {
        var handler = _container.GetInstance<ICommandHandler<TCommand, TResponse>>();
        return handler.HandleAsync(command, cancellationToken);
    }

    public Task<IResult> ExecuteCommand<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
        {
            var handler = _container.GetInstance<ICommandHandler<TCommand>>();
            return handler.HandleAsync(command, cancellationToken);
        }
    }

    public Task ExecuteEvent<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        using (AsyncScopedLifestyle.BeginScope(_container))
        {
            var handlers = _container.GetAllInstances<IEventHandler<TEvent>>();
            return Task.WhenAll(handlers.Select(x => x.HandleAsync(@event, cancellationToken)));
        }
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