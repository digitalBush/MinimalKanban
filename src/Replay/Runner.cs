using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Features.Variance;
using Denormalizers;
using Domain;
using Infrastructure.Persistence;
using NEventStore;

namespace Replay
{
    public class Runner
    {
        IContainer _container;

        public Runner()
        {
            _container = BuildContainer();
            Denormalizer.UseContainer(_container.BeginLifetimeScope());
        }

        public IStateRepository Rebuild()
        {
            var eventStore = _container.Resolve<IStoreEvents>();
            var commits = eventStore.Advanced.GetFrom(null);
            foreach (var commit in commits)
            {
                foreach (var @event in commit.Events)
                {
                    DispatchEvent(@event.Body as dynamic);
                }
            }
            //TODO: Dump InMemory to <T> passed to method?
            return _container.Resolve<IStateRepository>();
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

        static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof (Denormalizer<>).Assembly)
                .AsImplementedInterfaces();

            //Replay runs in memory, then flushed to disk
            builder.RegisterType<InMemoryStateRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterModule<EventStoreModule>();
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterType<PlaybackUIEventDispatcher>()
                .AsImplementedInterfaces()
                .SingleInstance();
            return builder.Build();
        }
    }
}
