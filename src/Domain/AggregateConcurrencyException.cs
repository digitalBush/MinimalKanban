using System;
using System.Collections.Generic;

namespace Domain
{
    public class AggregateConcurrencyException : Exception
    {
        public AggregateConcurrencyException(Type type, Guid id, List<IEvent> uncommitted, List<IEvent> committed)
        {
            throw new NotImplementedException();
        }
    }
}