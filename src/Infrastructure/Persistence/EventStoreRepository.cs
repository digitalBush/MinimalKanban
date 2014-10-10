using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Domain;
using NEventStore;

namespace Infrastructure.Persistence
{
    public class EventStoreRepository<T> : IDisposable, IRepository<T> where T : AggregateRoot
    {
        private readonly IStoreEvents _events;
        private readonly IDictionary<Guid, IEventStream> _streams = new ConcurrentDictionary<Guid, IEventStream>();



        public EventStoreRepository(IStoreEvents events)
        {
            _events = events;

        }

        public Task<T> Get(Guid id)
        {
            return Task.Run(() =>
            {

                var aggregate = (T)Activator.CreateInstance(typeof(T), true);
                var stream = _events.OpenStream(aggregate.Id);

                try
                {
                    if (stream.CommitSequence == 0)
                        return null;

                    //no aggregate with this id has committed any events.
                    if (stream.StreamRevision == 0) return null;

                    aggregate.LoadFromHistory(stream.CommittedEvents.Select(x => x.Body).OfType<IEvent>());
                    aggregate.Version = stream.StreamRevision;
                }
                finally
                {
                    stream.Dispose();
                }

                return aggregate;

            });
        }

        public Task Save(T aggregate)
        {
            return Task.Run(() =>
            {
                var stream = _events.OpenStream(aggregate.Id, aggregate.Version);

                var commitEventCount = stream.CommittedEvents.Count;

                foreach (var @event in aggregate.GetUncommittedEvents())
                {
                    stream.Add(new EventMessage { Body = @event });
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

                    stream.ClearChanges();
                    throw new AggregateConcurrencyException(typeof(T), aggregate.Id, uncommitted, committed);

                }
            });
        }

        public void Dispose()
        {
            foreach (var keyedStream in _streams)
            {
                keyedStream.Value.Dispose();
            }
        }
    }
}