using System.Reflection;
using CommuniGate.Container.Configurations;
using CommuniGate.Middlewares;

namespace CommuniGate.EventSourcing.Configurations;

public class EventSourcingConfiguration : BaseSimpleInjectorContainerConfiguration
{
    public override void Register(SimpleInjector.Container container, Assembly[] assemblies)
    {
        container.Register(typeof(IEventHandler<,>), assemblies);
        RegisterHandlers(container, typeof(IEventPipelineMiddleware<,>), assemblies, new []
        {
            typeof(ExceptionHandlingMiddleware<,>),
            typeof(ExceptionHandlingMiddleware<>)
        });
    }
}