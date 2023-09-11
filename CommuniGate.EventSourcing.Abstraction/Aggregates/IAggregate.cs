using CommuniGate.Repository.Abstraction;

namespace CommuniGate.EventSourcing.Abstraction.Aggregates;

public interface IAggregate : IEntity
{
    long? Version { get; }
}