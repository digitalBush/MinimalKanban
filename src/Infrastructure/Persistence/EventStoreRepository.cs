using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using NEventStore;

namespace Infrastructure.Persistence
{
    public class EventStoreRepository<T> : IRepository<T> where T : AggregateRoot
    {
        private readonly IStoreEvents _events;

        public EventStoreRepository(IStoreEvents events)
        {
            _events = events;
        }

        public Task<T> Get(Guid id)
        {
            return Task.Run(() =>{
                using (var stream = _events.OpenStream(id, 0, Int32.MaxValue))
                {
                    if (stream.CommitSequence == 0)
                        return null;
                    var aggregate = Activator.CreateInstance(typeof (T), true) as T;
                    aggregate.LoadFromHistory(stream.CommittedEvents.Select(x => x.Body).OfType<IEvent>());
                    return aggregate;
                }
            });
        }

        public Task Save(T aggregate)
        {
            return Task.Run(() =>{
                using (var stream = _events.OpenStream(aggregate.Id, 0, Int32.MaxValue))
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

                    }
                    catch (ConcurrencyException ex)
                    {
                        var uncommitted = stream.UncommittedEvents.Select(x => x.Body as IEvent).ToList();
                        var committed =
                            stream.CommittedEvents.Skip(commitEventCount).Select(x => x.Body as IEvent).ToList();
                        throw new AggregateConcurrencyException(typeof (T), aggregate.Id, uncommitted, committed);
                    }
                }
            });
        }
    }
}