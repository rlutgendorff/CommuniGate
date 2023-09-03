namespace CommuniGate.Containers;

public interface ICommuniGateContainer
{
    object GetInstance(Type serviceType);
    IEnumerable<object> GetAllInstances(Type serviceType);
    IEnumerable<TService> GetAllInstances<TService>() where TService : class;

    ICommuniGateScope CreateScope();
}