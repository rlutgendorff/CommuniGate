using CommuniGate.Results;

namespace CommuniGate.Queries;

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<IResult<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}