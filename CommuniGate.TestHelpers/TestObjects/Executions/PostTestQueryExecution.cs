using CommuniGate.Middlewares;
using CommuniGate.Results;
using CommuniGate.TestHelpers.TestObjects.Handlers;

namespace CommuniGate.TestHelpers.TestObjects.Executions;

public class PostTestQueryExecution : IPostExecution<TestQuery, IResult<string>>
{
    public void Process(TestQuery request, IResult<string> response)
    {
        if (response.Value == "Hello world")
            throw new ApplicationException("Exception");
    }
}