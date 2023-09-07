using System.Reflection;
using CommuniGate.Container.Configurations;
using CommuniGate.Events;
using CommuniGate.EventSourcing.Abstraction;
using CommuniGate.EventSourcing.Abstraction.Middlewares;
using CommuniGate.Middlewares;

namespace CommuniGate.EventSourcing.Configurations;

public class EventSourcingConfiguration : BaseSimpleInjectorContainerConfiguration
{
    public override void Register(SimpleInjector.Container container, Assembly[] assemblies)
    {
        var excluded = new[]
        {
            typeof(ExceptionHandlingMiddleware<,>),
            typeof(ExceptionHandlingMiddleware<>)
        };

        container.Register(typeof(IEventHandler<,>), assemblies);

        container.Collection.Register(typeof(IEventNotificationHandler<>), assemblies);
        RegisterHandlers(container, typeof(IEventPipelineMiddleware<>), assemblies, excluded);
        RegisterHandlers(container, typeof(IEventPipelineMiddleware<,>), assemblies, new []
        {
            typeof(ExceptionHandlingMiddleware<,>),
            typeof(ExceptionHandlingMiddleware<>)
        });
    }
}