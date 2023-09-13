using CommuniGate.Abstraction.Commands;
using CommuniGate.Commands;
using CommuniGate.Results;

namespace CommuniGate.TestHelpers.TestObjects.Handlers;

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