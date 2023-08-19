using CommuniGate.Events;

namespace CommuniGate.Tests.TestObjects;

public class TestEvent : IEvent
{}

public class TestEventHandler : IEventHandler<TestEvent>
{
    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

public class TestEventHandler2 : IEventHandler<TestEvent>
{
    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}