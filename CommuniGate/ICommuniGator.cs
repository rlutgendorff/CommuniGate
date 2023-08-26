using CommuniGate.Commands;
using CommuniGate.Events;
using CommuniGate.Queries;
using CommuniGate.Results;

namespace CommuniGate;

public interface ICommuniGator
{
    Task<IResult<TResponse>> Execute<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    Task<IResult<TResponse>> Execute<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    Task<IResult> Execute(ICommand command, CancellationToken cancellationToken = default);
    Task Execute(IEvent @event, CancellationToken cancellationToken = default);
}