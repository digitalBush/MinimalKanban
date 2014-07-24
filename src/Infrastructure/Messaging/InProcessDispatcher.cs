using System.Collections.Generic;
using Autofac;
using Domain;
using NEventStore;
using NEventStore.Dispatcher;

namespace Infrastructure.Messaging
{
    public class InProcessDispatcher : IDispatchCommits
    {
        readonly ILifetimeScope _container;

        public InProcessDispatcher(ILifetimeScope container)
        {
            _container = container;
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public void Dispatch(ICommit commit)
        {
            foreach (var @event in commit.Events)
            {
                DispatchEvent(@event.Body as dynamic);
            }
        }

        void DispatchEvent<T>(T @event) where T : IEvent
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var handlers = scope.Resolve<IEnumerable<IHandle<T>>>();
                foreach (var handler in handlers)
                {
                    handler.Handle(@event);
                }
            }
        }
    }
}