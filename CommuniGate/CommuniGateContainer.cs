using System.Reflection;
using CommuniGate.Commands;
using CommuniGate.Events;
using CommuniGate.Middlewares;
using CommuniGate.Queries;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CommuniGate;

public interface ICommuniGateContainer
{
    object GetInstance(Type serviceType);
    IEnumerable<object> GetAllInstances(Type serviceType);
    IEnumerable<TService> GetAllInstances<TService>() where TService : class;

    ICommuniGateScope CreateScope();
}

public interface ICommuniGateScope : IDisposable
{

}

public class CommuniGateScope : ICommuniGateScope
{
    private readonly Scope _scope;

    public CommuniGateScope(Scope scope)
    {
        _scope = scope;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}

internal class CommuniGateContainer : ICommuniGateContainer
{
    internal Container Container { get; } = Create();

    internal void Init(Assembly[] assembliesToScan)
    {
        assembliesToScan = AddThisAssemblyToAssemblies(assembliesToScan);

        Container.Register<ICommuniGator, CommuniGator>();
        Container.Register(typeof(IQueryHandler<,>), assembliesToScan);
        Container.Register(typeof(ICommandHandler<>), assembliesToScan);
        Container.Register(typeof(ICommandHandler<,>), assembliesToScan);

        Container.Collection.Register(typeof(IEventNotificationHandler<>), assembliesToScan);

        RegisterHandlers(typeof(IEventPipelineMiddleware<>), assembliesToScan);
        RegisterHandlers(typeof(IPipelineMiddleware<>), assembliesToScan);
        RegisterHandlers(typeof(IPipelineMiddleware<,>), assembliesToScan);
        RegisterHandlers(typeof(IPreExecution<>), assembliesToScan);
        RegisterHandlers(typeof(IPostExecution<,>), assembliesToScan);
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

    private void RegisterHandlers(Type collectionType, IEnumerable<Assembly> assemblies)
    {
        var handlerTypes = Container.GetTypesToRegister(collectionType, assemblies, new TypesToRegisterOptions
        {
            IncludeGenericTypeDefinitions = true,
            IncludeComposites = false,
        });

        handlerTypes = handlerTypes.Where(x =>
            x != typeof(ExceptionHandlingMiddleware<,>) && x != typeof(ExceptionHandlingMiddleware<>));

        Container.Collection.Register(collectionType, handlerTypes);
    }

    private Assembly[] AddThisAssemblyToAssemblies(Assembly[] assembliesToScan)
    {
        var newArray = new Assembly[assembliesToScan.Length + 1];
        Array.Copy(assembliesToScan, newArray, assembliesToScan.Length);
        newArray[^1] = GetType().Assembly;

        return newArray;
    }

    private static Container Create()
    {
        var container = new Container();
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        return container;
    }

    
}