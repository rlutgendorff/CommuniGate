using CommuniGate.Events;
using CommuniGate.EventSourcing.Abstraction;
using CommuniGate.EventSourcing.Abstraction.Aggregates;
using CommuniGate.EventSourcing.Extensions;

namespace CommuniGate.EventSourcing.Aggregates;

public class Aggregate : IAggregate
{
    public Guid Id { get; set; }

    public long? Version { get; set; }

    internal InternalChangeTracker ChangeTracker { get; private set; }

    //TODO
    //public void Delete(DeleteNotification delete)
    //{
    //    ChangeTracker.AddUncommittedEvent(delete, new EventMetadata { Id = Id, TypeName = delete.GetType().AssemblyQualifiedName });
    //}

    protected void AddEvent(IEvent @event)
    {
        var metadata = new EventMetadata { Id = Id, TypeName = @event.GetType().AssemblyQualifiedName };

        var wrapper = ChangeTracker.AddUncommittedEvent(@event, metadata);
        ChangeTracker.Apply(wrapper);
    }

    internal static void AddChangeTracker<TEntity>(TEntity entity, InternalChangeTracker tracker)
        where TEntity : Aggregate
    {
        entity.ChangeTracker = tracker;
    }

    internal class InternalChangeTracker
    {
        private readonly IList<EventWrapper> _events;
        private readonly ICommuniGator _processor;
        private readonly Aggregate _entity;

        public InternalChangeTracker(ICommuniGator processor, Aggregate entity)
        {
            _entity = entity;
            _processor = processor;
            _events = new List<EventWrapper>();
        }

        public void Apply(EventWrapper @event, bool shouldValidate = true)
        {
            _processor.Execute(_entity, @event.Event);

            //TODO
            //ValidationStates validation = _processor.Execute((dynamic)_entity, (dynamic)@event.Event, shouldValidate);

            //if (validation.IsValid)
            //{
            //    _entity.Version = @event.AggregateVersion;
            //}
            //else
            //{
            //    throw new ValidationStatesException(validation);
            //}
        }

        public IEnumerable<EventWrapper> GetUncommittedEvents()
        {
            return _events;
        }

        public void ClearUncommittedEvents()
        {
            _events.Clear();
        }

        public EventWrapper AddUncommittedEvent(IEvent command, EventMetadata metadata)
        {
            var wrapper = new EventWrapper
            {
                AggregateId = _entity.Id,
                AggregateVersion = ++_entity.Version ?? 0,
                Event = command,
                Metadata = metadata
            };

            _events.Add(wrapper);
            return wrapper;
        }
    }
}