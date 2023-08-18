namespace CommuniGate.Middlewares;

public interface IPreProcessor<in TRequest>
    where TRequest : class, ICommunication
{

    Task Process(TRequest request, CancellationToken cancellation = default);
}