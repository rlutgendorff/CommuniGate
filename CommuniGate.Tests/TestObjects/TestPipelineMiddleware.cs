using CommuniGate.Middlewares;
using CommuniGate.Queries;

namespace CommuniGate.Tests.TestObjects;

public class TestPipelineMiddleware : IPipelineMiddleware<IQuery<string>, string>
{
    public Task<IResult<string>> Handle(IQuery<string> request, RequestHandlerDelegate<string> next, CancellationToken cancellationToken)
    {
        return next.Invoke();
    }
}