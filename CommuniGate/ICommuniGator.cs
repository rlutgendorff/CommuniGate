using CommuniGate.Commands;
using CommuniGate.Queries;

namespace CommuniGate;

public interface ICommuniGator
{
    Task<IResult<TResponse>> Execute<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);

    Task<IResult<TResponse>> Execute<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);

    Task<IResult> Execute(ICommand command, CancellationToken cancellationToken = default);
}