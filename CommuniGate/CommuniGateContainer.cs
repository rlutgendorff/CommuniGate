using System.Reflection;
using CommuniGate.Commands;
using CommuniGate.Events;
using CommuniGate.Middlewares;
using CommuniGate.Queries;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CommuniGate;

internal class CommuniGateContainer
{
    internal Container Container { get; } = Create();

    internal void Init(Assembly[] assembliesToScan)
    {
        assembliesToScan = AddThisAssemblyToAssemblies(assembliesToScan);

        Container.Register<ICommuniGator, CommuniGator>();
        Container.Register(typeof(IQueryHandler<,>), assembliesToScan);
        Container.Register(typeof(ICommandHandler<>), assembliesToScan);
        Container.Register(typeof(ICommandHandler<,>), assembliesToScan);

        Container.Collection.Register(typeof(IEventHandler<>), assembliesToScan);
        Container.Collection.Register(typeof(IEventPipelineMiddleware<>), assembliesToScan);
        Container.Collection.Register(typeof(IPipelineMiddleware<>), assembliesToScan);
        Container.Collection.Register(typeof(IPipelineMiddleware<,>), assembliesToScan);
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