namespace CommuniGate.EventSourcing.Abstraction.ServiceBus;

public class Message
{
    public string Data { get; set; }
    public EventMetadata Metadata { get; set; }
}