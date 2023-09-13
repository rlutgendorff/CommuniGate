using CommuniGate.Commands;
using CommuniGate.Middlewares;
using CommuniGate.Queries;
using System.Reflection;
using CommuniGate.Abstraction.Commands;
using CommuniGate.Container.Configurations;

namespace CommuniGate.Configuration;

public class SimpleInjectorContainerConfiguration : BaseSimpleInjectorContainerConfiguration
{
    public override void Register(SimpleInjector.Container container, Assembly[] assemblies)
    {
        container.Register<ICommuniGator, CommuniGator>();
        container.Register(typeof(IQueryHandler<,>), assemblies);
        container.Register(typeof(ICommandHandler<>), assemblies);
        container.Register(typeof(ICommandHandler<,>), assemblies);

        var excluded = new[]
        {
            typeof(ExceptionHandlingMiddleware<,>),
            typeof(ExceptionHandlingMiddleware<>)
        };

        
        RegisterHandlers(container, typeof(IPipelineMiddleware<>), assemblies, excluded);
        RegisterHandlers(container, typeof(IPipelineMiddleware<,>), assemblies, excluded);
        RegisterHandlers(container, typeof(IPreExecution<>), assemblies, excluded);
        RegisterHandlers(container, typeof(IPostExecution<,>), assemblies, excluded);
    }
}