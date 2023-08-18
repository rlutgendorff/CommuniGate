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

    internal void Init(Assembly[] assemblies)
    {
        Container.Register<ICommuniGator, CommuniGator>();
        Container.Register(typeof(IQueryHandler<,>), assemblies);
        Container.Register(typeof(ICommandHandler<>), assemblies);
        Container.Register(typeof(ICommandHandler<,>), assemblies);

        Container.Collection.Register(typeof(IEventHandler), assemblies);
        Container.Collection.Register(typeof(IPipelineMiddleware<,>), assemblies);
        Container.Collection.Register(typeof(IPostProcessor<>), assemblies);
        Container.Collection.Register(typeof(IPreProcessor<>), assemblies);

        
    }

    //public TType GetInstance<TType>()
    //    where TType : class
    //{
    //    return _container.GetInstance<TType>();
    //}

    //public object GetInstance(Type type)
    //{
    //    return _container.GetInstance(type);
    //}

    //public IEnumerable<TType> GetInstances<TType>()
    //    where TType : class
    //{
    //    return _container.GetAllInstances<TType>();
    //}

    //public IEnumerable<object> GetInstances(Type type)
    //{
    //    return _container.GetAllInstances(type);
    //}

    private static Container Create()
    {
        var container = new Container();
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        return container;
    }
}