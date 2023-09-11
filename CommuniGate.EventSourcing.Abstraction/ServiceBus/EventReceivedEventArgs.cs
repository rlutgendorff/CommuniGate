namespace CommuniGate.EventSourcing.Abstraction.ServiceBus;

public class EventReceivedEventArgs : EventArgs
{
    public EventReceivedEventArgs(Message message)
    {
        Message = message;
    }

    public Message Message { get; }
}