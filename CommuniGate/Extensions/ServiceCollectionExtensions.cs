using System.Reflection;
using CommuniGate.Container.Abstraction;
using CommuniGate.Container.Extensions;
using CommuniGate.Containers;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace CommuniGate.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommuniGate(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddSingleton<ICommuniGator, CommuniGator>();
        services.AddCommuniGateContainer(assemblies);

        return services;
    }
}