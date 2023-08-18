using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace CommuniGate.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddCommuniGate(this IServiceCollection services, Assembly[] assemblies)
    {
        var container = new CommuniGateContainer();

        services.AddSingleton<ICommuniGator, CommuniGator>();
        services.AddSingleton(container);
        

        services.AddSimpleInjector(container.Container);
        container.Init(assemblies);

        return services;
    }
}