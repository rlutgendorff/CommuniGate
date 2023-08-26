using CommuniGate.Commands;
using CommuniGate.Events;
using CommuniGate.Queries;
using CommuniGate.Results;

namespace CommuniGate;

public interface ICommuniGator
{
    Task<IResult<TResponse>> ExecuteQuery<TQuery, TResponse>(TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : class, IQuery<TResponse>;
 

     Task<IResult<TResponse>> ExecuteCommand<TCommand, TResponse>(TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : class, ICommand<TResponse>;

    Task<IResult> ExecuteCommand<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand;

    Task ExecuteEvent<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
}