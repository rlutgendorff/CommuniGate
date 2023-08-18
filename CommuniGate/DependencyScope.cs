using SimpleInjector;

namespace CommuniGate;

public class DependencyScope : IDependencyScope
{
    private readonly Scope _scope;

    public DependencyScope(Scope scope)
    {
        _scope = scope;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}