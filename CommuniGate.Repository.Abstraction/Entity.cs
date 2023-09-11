namespace CommuniGate.Repository.Abstraction;

public abstract class Entity : IEntity
{
    public Guid Id { get; set; }
}