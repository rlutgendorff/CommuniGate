using System.Reflection;

namespace CommuniGate.Containers.Configurations;

public interface IContainerConfiguration<in TContainer>
{
    void Register(TContainer container, Assembly[] assemblies);
}