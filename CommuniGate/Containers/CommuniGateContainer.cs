using System.Reflection;
using CommuniGate.Containers.Configurations;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CommuniGate.Containers;

internal class CommuniGateContainer : ICommuniGateContainer
{
    internal Container Container { get; } = Create();

    internal void Init(Assembly[] assembliesToScan)
    {
        var configurator = new ContainerConfigurator();
        configurator.Configure<Container>(Container, assembliesToScan);
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
    

    private static Container Create()
    {
        var container = new Container();
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        return container;
    }


}