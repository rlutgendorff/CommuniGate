using System.Reflection;
using CommuniGate.Container.Abstraction;
using CommuniGate.Container.Configurations;
using SimpleInjector.Lifestyles;

namespace CommuniGate.Containers;

public class CommuniGateContainer : ICommuniGateContainer
{
    internal SimpleInjector.Container Container { get; } = Create();

    internal void Init(Assembly[] assembliesToScan)
    {
        ContainerConfigurator.Configure<SimpleInjector.Container>(Container, assembliesToScan);
    }

    public object GetInstance(Type serviceType)
    {
        return Container.GetInstance(serviceType);
    }

    public IEnumerable<object> GetAllInstances(Type serviceType)
    {
        return Container.GetAllInstances(serviceType);
    }

    public IEnumerable<TService> GetAllInstances<TService>() where TService : class
    {
        return Container.GetAllInstances<TService>();
    }

    public ICommuniGateScope CreateScope()
    {
        return new CommuniGateScope(AsyncScopedLifestyle.BeginScope(Container));
    }
    

    private static SimpleInjector.Container Create()
    {
        var container = new SimpleInjector.Container();
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        return container;
    }


}