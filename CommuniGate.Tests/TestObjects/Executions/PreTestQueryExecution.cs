using CommuniGate.Middlewares;
using CommuniGate.Tests.TestObjects.Handlers;

namespace CommuniGate.Tests.TestObjects.Executions;

public class PreTestQueryExecution : IPreExecution<TestQuery>
{
    public void Process(TestQuery request)
    {
        request.Name = "world";
    }
}