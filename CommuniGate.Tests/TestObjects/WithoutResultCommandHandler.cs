using CommuniGate.Commands;
using CommuniGate.Middlewares;
using CommuniGate.Results;

namespace CommuniGate.Tests.TestObjects;

public class WithoutResultCommand : ICommand
{

}

public class WithoutResultCommandHandler : ICommandHandler<WithoutResultCommand>
{
    public Task<IResult> HandleAsync(WithoutResultCommand command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IResult>(new Result());
    }
}