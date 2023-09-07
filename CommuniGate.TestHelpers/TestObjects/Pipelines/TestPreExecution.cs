using CommuniGate.Middlewares;

namespace CommuniGate.TestHelpers.TestObjects.Pipelines;

public class TestCommandPreExecution<TRequest> : IPreExecution<TRequest>
{
    public void Process(TRequest request)
    {
    }
}