namespace CommuniGate.Events;

public interface IEventHandler
{
    Task<IResult> HandleAsync(IEvent @event, CancellationToken cancellationToken = default);
}