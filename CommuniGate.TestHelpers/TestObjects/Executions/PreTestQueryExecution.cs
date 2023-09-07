using CommuniGate.Middlewares;
using CommuniGate.TestHelpers.TestObjects.Handlers;

namespace CommuniGate.TestHelpers.TestObjects.Executions;

public class PreTestQueryExecution : IPreExecution<TestQuery>
{
    public void Process(TestQuery request)
    {
        request.Name = "world";
    }
}