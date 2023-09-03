using CommuniGate.Events;

namespace CommuniGate.TestHelpers.TestObjects.Handlers;

public class TestEvent : IEvent
{}

public class TestEventNotificationHandler : IEventNotificationHandler<TestEvent>
{
    public static event EventHandler? OnHandling;


    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
    {
        OnHandling?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }
}

public class TestEventNotificationHandler2 : IEventNotificationHandler<TestEvent>
{
    public static event EventHandler? OnHandling;

    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
    {
        OnHandling?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }
}