namespace CommuniGate.EventSourcing.Abstraction.ServiceBus;

public interface IEventReceiver
{
    void Subscibe(Func<EventReceivedEventArgs, Task> action);
}