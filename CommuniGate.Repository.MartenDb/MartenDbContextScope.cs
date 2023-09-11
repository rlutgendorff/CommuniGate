using CommuniGate.Repository.Abstraction.Databases;
using Marten;

namespace CommuniGate.Repository.MartenDb;

public class MartenDbContextScope : IDatabaseContextScope
{
    private readonly IDocumentSession _session;

    public MartenDbContextScope(IDocumentSession session)
    {
        _session = session;
    }

    public void Dispose()
    {
        _session.SaveChanges();
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(_session.SaveChangesAsync());
    }
}