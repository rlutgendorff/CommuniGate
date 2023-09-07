using CommuniGate.Container.Extensions;

namespace CommuniGate.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider UseCommuniGate(this IServiceProvider serviceProvider)
    {
        serviceProvider.UseCommuniGateContainer();

        return serviceProvider;
    }
}