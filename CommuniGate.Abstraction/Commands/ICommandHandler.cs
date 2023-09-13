using CommuniGate.Commands;
using CommuniGate.Results;

namespace CommuniGate.Abstraction.Commands;

public interface ICommandHandler<in TRequest>
    where TRequest : class, ICommand
{
    public Task<IResult> HandleAsync(TRequest command, CancellationToken cancellationToken = default);
    
}

public interface ICommandHandler<in TRequest, TResponse>
    where TRequest : class, ICommand<TResponse>
{
    public Task<IResult<TResponse>> HandleAsync(TRequest command, CancellationToken cancellationToken = default);
}