using SimpleInjector;

namespace CommuniGate.Containers;

public class CommuniGateScope : ICommuniGateScope
{
    private readonly Scope _scope;

    public CommuniGateScope(Scope scope)
    {
        _scope = scope;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}