using System.Reflection;
using CommuniGate.Container.Abstraction.Configurations;
using SimpleInjector;

namespace CommuniGate.Container.Configurations;

public abstract class BaseSimpleInjectorContainerConfiguration : IContainerConfiguration<SimpleInjector.Container>
{
    public abstract void Register(SimpleInjector.Container container, Assembly[] assemblies);

    protected static void RegisterHandlers(SimpleInjector.Container container, Type collectionType, IEnumerable<Assembly> assemblies , Type[] exclude)
    {
        var handlerTypes = container.GetTypesToRegister(collectionType, assemblies, new TypesToRegisterOptions
        {
            IncludeGenericTypeDefinitions = true,
            IncludeComposites = false,
        });

        handlerTypes = handlerTypes.Except(exclude);
           

        container.Collection.Register(collectionType, handlerTypes);
    }
}