using CommuniGate.Events;
using CommuniGate.EventSourcing.Abstraction;
using CommuniGate.EventSourcing.Abstraction.Aggregates;

namespace CommuniGate.EventSourcing.Tests.TestObjects;

public class TestEntity : IAggregate
{
    public string Status { get; set; } = "No Change";
    public Guid Id { get; set; }
    public long? Version { get; }
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