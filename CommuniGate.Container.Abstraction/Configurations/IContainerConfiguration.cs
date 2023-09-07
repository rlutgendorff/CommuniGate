using System.Reflection;

namespace CommuniGate.Container.Abstraction.Configurations;

public interface IContainerConfiguration<in TContainer>
{
    void Register(TContainer container, Assembly[] assemblies);
}