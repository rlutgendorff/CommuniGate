using CommuniGate.Middlewares;

namespace CommuniGate.TestHelpers.TestObjects.Pipelines;

public class TestPostExecution<TRequest, TResponse> : IPostExecution<TRequest, TResponse>
{
    public void Process(TRequest request, TResponse response)
    {
        
    }
}