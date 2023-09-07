namespace CommuniGate.EventSourcing.Abstraction.ServiceBus;

public interface IEventPublisher
{
    public void Publish(EventWrapper eventWrapper);
}