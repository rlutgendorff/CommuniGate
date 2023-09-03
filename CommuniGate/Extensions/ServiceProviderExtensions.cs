using CommuniGate.Containers;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace CommuniGate.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider UseCommuniGate(this IServiceProvider serviceProvider)
    {
        var container = serviceProvider.GetService<CommuniGateContainer>()?.Container;

        serviceProvider.UseSimpleInjector(container);

        return serviceProvider;
    }
}