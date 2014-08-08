using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using NEventStore;

namespace Infrastructure.Persistence
{
    public class EventStoreRepository<T> : IRepository<T> where T : AggregateRoot
    {
        private readonly IStoreEvents _events;
        readonly ConflictDetector _conflictDetector;

        public EventStoreRepository(IStoreEvents events, ConflictDetector conflictDetector)
        {
            _events = events;
            _conflictDetector = conflictDetector;
        }

        public Task<T> Get(Guid id)
        {
            return Task.Run(() =>{
                using (var stream = _events.OpenStream(id, 0))
                {
                    if (stream.CommitSequence == 0)
                        return null;
                    var aggregate = (T)Activator.CreateInstance(typeof (T), true);
                    aggregate.LoadFromHistory(stream.CommittedEvents.Select(x => x.Body).OfType<IEvent>());
                    return aggregate;
                }
            });
        }

        public Task Save(T aggregate)
        {
            return Task.Run(() =>{
                using (var stream = _events.OpenStream(aggregate.Id))
                {
                    while (true)
                    {
                        var commitEventCount = stream.CommittedEvents.Count;

                        foreach (var @event in aggregate.GetUncommittedEvents())
                        {
                            stream.Add(new EventMessage {Body = @event});
                        }
                        try
                        {
                            stream.CommitChanges(Guid.NewGuid());
                            aggregate.MarkChangesAsCommitted();
                            return;

                        }
                        catch (ConcurrencyException ex)
                        {
                            var uncommitted = stream.UncommittedEvents.Select(x => x.Body as IEvent).ToList();
                            var committed =
                                stream.CommittedEvents.Skip(commitEventCount).Select(x => x.Body as IEvent).ToList();

                            if (_conflictDetector.HasConflicts(committed,uncommitted))
                                throw new AggregateConcurrencyException(typeof (T), aggregate.Id, uncommitted, committed);

                            //We'll start over again
                            stream.ClearChanges();
                        }
                    }
                }
            });
        }

        bool IsAllowed(List<IEvent> committed, List<IEvent> uncommitted)
        {
            throw new NotImplementedException();
        }
    }
}