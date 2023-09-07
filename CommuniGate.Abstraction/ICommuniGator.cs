using CommuniGate.Commands;
using CommuniGate.Container.Abstraction;
using CommuniGate.Queries;
using CommuniGate.Results;

namespace CommuniGate;

public interface ICommuniGator
{
    Task<IResult<TResponse>> Execute<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    Task<IResult<TResponse>> Execute<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    Task<IResult> Execute(ICommand command, CancellationToken cancellationToken = default);

    Task Execute(Func<ICommuniGateContainer, Task> func);

    Task<IResult> Execute(Func<ICommuniGateContainer, Task<IResult>> func);
    Task<IResult<TResponse>> Execute<TResponse>(Func<ICommuniGateContainer, Task<IResult<TResponse>>> func);

}