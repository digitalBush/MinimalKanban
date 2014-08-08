using System;
using System.Collections.Generic;

namespace Domain
{
    public class AggregateConcurrencyException : Exception
    {
        public Type Type { get; private set; }
        public Guid Id { get; private set; }
        public List<IEvent> UncommittedEvents { get; private set; }
        public List<IEvent> CommittedEvents { get; private set; }

        public AggregateConcurrencyException(Type type, Guid id, List<IEvent> uncommittedEvents, List<IEvent> committedEvents)
        {
            Type = type;
            Id = id;
            UncommittedEvents = uncommittedEvents;
            CommittedEvents = committedEvents;
        }
    }
}