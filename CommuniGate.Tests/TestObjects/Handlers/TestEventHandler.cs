using CommuniGate.Events;

namespace CommuniGate.Tests.TestObjects;

public class TestEvent : IEvent
{}

public class TestEventHandler : IEventHandler<TestEvent>
{
    public static event EventHandler? OnHandling;


    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
    {
        OnHandling?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }
}

public class TestEventHandler2 : IEventHandler<TestEvent>
{
    public static event EventHandler? OnHandling;

    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
    {
        OnHandling?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }
}