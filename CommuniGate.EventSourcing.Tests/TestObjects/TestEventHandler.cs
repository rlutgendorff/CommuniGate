using CommuniGate.Events;

namespace CommuniGate.EventSourcing.Tests.TestObjects;

public class TestEntity
{
    public string Status { get; set; } = "No Change";
}

public class TestEntityEvent : IEvent
{
    public string Status { get; set; } = "Change";
}

public class TestEventHandler : IEventHandler<TestEntity, TestEntityEvent>
{
    public Task HandleAsync(TestEntity entity, TestEntityEvent @event, CancellationToken cancellationToken = default)
    {
        entity.Status = @event.Status;
        return Task.CompletedTask;
    }
}