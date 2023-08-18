﻿namespace CommuniGate.Queries;

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<IResult<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}