﻿using CommuniGate.Results;
using SimpleInjector;

namespace CommuniGate.Middlewares;

public class PostExecutionMiddleware<TRequest> : IPipelineMiddleware<TRequest>
{
    private readonly SimpleInjector.Container _container;

    public PostExecutionMiddleware(SimpleInjector.Container container)
    {
        _container = container;
    }

    public Task<IResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken)
    {
        var result = next.Invoke();

        var cType = typeof(IPostExecution<,>).MakeGenericType(request!.GetType(), typeof(Task<IResult>));
        var processors = _container.GetAllInstances(cType).Cast<dynamic>().ToList();

        processors.ForEach(x => x.Process((dynamic)request, (dynamic)result));

        return result;
    }
}

public class PostExecutionMiddleware<TRequest, TResponse> : IPipelineMiddleware<TRequest, TResponse>
{
    private readonly SimpleInjector.Container _container;

    public PostExecutionMiddleware(SimpleInjector.Container container)
    {
        _container = container;
    }

    public async Task<IResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next.Invoke();

        var cType = typeof(IPostExecution<,>).MakeGenericType(request!.GetType(), typeof(IResult<TResponse>));
        var processors = _container.GetAllInstances(cType).Cast<dynamic>().ToList();

        processors.ForEach(x => x.Process((dynamic)request, (dynamic)result));

        return result;
    }
}

public class PostEventExecutionMiddleware<TEvent> : IEventPipelineMiddleware<TEvent>
{
    private readonly SimpleInjector.Container _container;

    public PostEventExecutionMiddleware(SimpleInjector.Container container)
    {
        _container = container;
    }

    public Task Handle(TEvent @event, EventHandlerDelegate next, CancellationToken cancellationToken)
    {
        var result = next.Invoke();

        var cType = typeof(IPostExecution<,>).MakeGenericType(@event!.GetType(), typeof(Task));
        var processors = _container.GetAllInstances(cType).Cast<dynamic>().ToList();

        processors.ForEach(x => x.Process((dynamic)@event, (dynamic)result));

        return result;
    }
}