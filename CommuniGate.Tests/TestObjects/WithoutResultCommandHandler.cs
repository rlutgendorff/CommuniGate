using CommuniGate.Commands;

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