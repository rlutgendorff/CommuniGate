namespace CommuniGate.Middlewares;

public interface IPostExecution<in TRequest, in TResponse>
{
    void Process(TRequest request, TResponse response);
}