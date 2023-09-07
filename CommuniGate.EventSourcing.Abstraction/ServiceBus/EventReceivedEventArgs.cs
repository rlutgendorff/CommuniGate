namespace CommuniGate.EventSourcing.Abstraction.ServiceBus;

public class EventReceivedEventArgs : EventArgs
{
    public Message Message { get; set; }
}