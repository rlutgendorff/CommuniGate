using CommuniGate.Container.Abstraction;
using CommuniGate.Containers;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace CommuniGate.Container.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommuniGateContainer(this IServiceCollection services, Assembly[] assemblies)
    {
        var container = new CommuniGateContainer();

        services.AddSingleton<ICommuniGateContainer>(_ => container);
        services.AddSingleton(container);

        services.AddSimpleInjector(container.Container);
        container.Init(assemblies);

        return services;
    }
}