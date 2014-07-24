using System;

namespace Domain
{
    public interface IEvent
    {
        Guid AggregateId { get; }
        Guid EventId { get; }
        DateTime Timestamp { get; }
    }

    public abstract class Event:IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid EventId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}