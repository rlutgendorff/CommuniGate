namespace CommuniGate.Middlewares;

public interface IPreExecution<in TRequest>
{
    void Process(TRequest request);
}