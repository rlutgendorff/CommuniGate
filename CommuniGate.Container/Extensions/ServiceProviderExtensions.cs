using CommuniGate.Container.Abstraction;
using CommuniGate.Containers;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace CommuniGate.Container.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider UseCommuniGateContainer(this IServiceProvider serviceProvider)
    {
        var container = serviceProvider.GetService<ICommuniGateContainer>();

        if (container is CommuniGateContainer cgContainer)
        {
            serviceProvider.UseSimpleInjector(cgContainer.Container);
        }

        return serviceProvider;
    }
}