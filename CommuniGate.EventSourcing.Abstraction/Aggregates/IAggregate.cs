namespace CommuniGate.EventSourcing.Abstraction.Aggregates;

public interface IAggregate 
{
    public Guid Id { get; set; }
    long? Version { get; }
}