using CommuniGate.Commands;

namespace CommuniGate.Tests.TestObjects;

public sealed class WithResultCommand : ICommand<int>
{
    public int Test { get; set; }
}

public sealed class WithResultCommandHandler : ICommandHandler<WithResultCommand, int>
{
    public Task<IResult<int>> HandleAsync(WithResultCommand commandHandler, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IResult<int>>(new Result<int>(commandHandler.Test + 1));
    }
}