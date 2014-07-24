using System;

namespace Domain
{
    public class HandlerNotFoundException : Exception
    {
        public IEvent Event { get; set; }

        public HandlerNotFoundException(IEvent @event)
        {
            Event = @event;
        }
    }
}