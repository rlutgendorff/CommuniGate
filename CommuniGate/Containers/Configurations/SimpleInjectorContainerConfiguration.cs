using CommuniGate.Commands;
using CommuniGate.Events;
using CommuniGate.Middlewares;
using CommuniGate.Queries;
using SimpleInjector;
using System.Reflection;

namespace CommuniGate.Containers.Configurations;

public abstract class BaseSimpleInjectorContainerConfiguration : IContainerConfiguration<Container>
{
    public abstract void Register(Container container, Assembly[] assemblies);

    protected void RegisterHandlers(Container container, Type collectionType, IEnumerable<Assembly> assemblies)
    {
        var handlerTypes = container.GetTypesToRegister(collectionType, assemblies, new TypesToRegisterOptions
        {
            IncludeGenericTypeDefinitions = true,
            IncludeComposites = false,
        });

        handlerTypes = handlerTypes.Where(x =>
            x != typeof(ExceptionHandlingMiddleware<,>) && x != typeof(ExceptionHandlingMiddleware<>));

        container.Collection.Register(collectionType, handlerTypes);
    }
}

public class SimpleInjectorContainerConfiguration : BaseSimpleInjectorContainerConfiguration
{
    public override void Register(Container container, Assembly[] assemblies)
    {
        container.Register<ICommuniGator, CommuniGator>();
        container.Register(typeof(IQueryHandler<,>), assemblies);
        container.Register(typeof(ICommandHandler<>), assemblies);
        container.Register(typeof(ICommandHandler<,>), assemblies);

        container.Collection.Register(typeof(IEventNotificationHandler<>), assemblies);

        RegisterHandlers(container, typeof(IEventPipelineMiddleware<>), assemblies);
        RegisterHandlers(container, typeof(IPipelineMiddleware<>), assemblies);
        RegisterHandlers(container, typeof(IPipelineMiddleware<,>), assemblies);
        RegisterHandlers(container, typeof(IPreExecution<>), assemblies);
        RegisterHandlers(container, typeof(IPostExecution<,>), assemblies);
    }
}