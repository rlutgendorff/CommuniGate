using CommuniGate.Queries;
using CommuniGate.Results;

namespace CommuniGate.Tests.TestObjects.Handlers;

public sealed class TestQuery : IQuery<string>
{
    public required string Name { get; set; }
}

public class TestQueryHandler : IQueryHandler<TestQuery, string>
{
    private readonly ITestService _testService;

    public TestQueryHandler(ITestService testService)
    {
        _testService = testService;
    }

    public Task<IResult<string>> HandleAsync(TestQuery query, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IResult<string>>(new Result<string>(_testService.Test(query.Name)));
    }
}