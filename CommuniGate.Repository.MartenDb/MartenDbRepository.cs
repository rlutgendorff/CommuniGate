using CommuniGate.Repository.Abstraction;
using CommuniGate.Repository.Abstraction.Databases;
using Marten;

namespace CommuniGate.Repository.MartenDb;

public class MartenDbRepository<TEntity> : IDatabaseRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly IDocumentSession _session;

    public MartenDbRepository(IDocumentStore store)
    {
        _session = store.LightweightSession();
    }

    public Task<TEntity?> GetByIdAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return _session.LoadAsync<TEntity>(entity.Id, cancellationToken);
    }

    public Task SaveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.Run(()=> _session.Store(entity), cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.Run(() => _session.Delete(entity), cancellationToken);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _session.Query<TEntity>();
    }

    public Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return Task.Run(() => _session.DeleteObjects(entities), cancellationToken);
    }

    public IDatabaseContextScope BeginScope()
    {
        return new MartenDbContextScope(_session);
    }
}