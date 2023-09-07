using System.Reflection;
using CommuniGate.Containers.Configurations;
using SimpleInjector;

namespace CommuniGate.EventSourcing.Configurations;

public class EventSourcingConfiguration : BaseSimpleInjectorContainerConfiguration
{
    public override void Register(Container container, Assembly[] assemblies)
    {
        container.Register(typeof(IEventHandler<,>), assemblies);
        RegisterHandlers(container, typeof(IEventPipelineMiddleware<,>), assemblies);
    }
}