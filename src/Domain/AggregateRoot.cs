using System;
using System.Collections.Generic;

namespace Domain
{
    public abstract class AggregateRoot
    {
        private readonly ICollection<IEvent> _uncommittedEvents = new List<IEvent>();

        public Guid Id { get; protected set; }
        protected internal int Version { get; set; }

        public ICollection<IEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public void MarkChangesAsCommitted()
        {
            _uncommittedEvents.Clear();
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var @event in history) ApplyEvent(@event);
        }

        private void ApplyEvent(IEvent @event)
        {
            EventRouter.Dispatch(this, @event);
            Version++;
        }

        protected void RaiseEvent(Event @event)
        {
            if (Version == 0)
            {
                if (@event.AggregateId == Guid.Empty)
                    @event.AggregateId = Guid.NewGuid(); //TODO: Combed Guid?

                this.Id = @event.AggregateId;
            }

            @event.AggregateId = this.Id;
            @event.Timestamp = DateTime.UtcNow;
            @event.EventId = Guid.NewGuid(); //TODO: Flake or combed guid?

            ApplyEvent(@event);
            _uncommittedEvents.Add(@event);
        }
    }
}
