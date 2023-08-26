using CommuniGate.Commands;
using CommuniGate.Results;

namespace CommuniGate.Tests.TestObjects.Handlers;

public class ExceptionCommand : ICommand
{

}

public class ExceptionCommandHandler : ICommandHandler<ExceptionCommand>
{
    public Task<IResult> HandleAsync(ExceptionCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}