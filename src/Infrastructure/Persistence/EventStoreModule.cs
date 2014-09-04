using Autofac;
using EventConverters;
using Infrastructure.Messaging;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;

namespace Infrastructure.Persistence
{
    public class EventStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var eventStore =
                builder.Register(container =>
                {
                    var wireup = Wireup.Init()
                        .LogToOutputWindow()
                        .UsingSqlPersistence("EventStore")
                        .WithDialect(new MsSqlDialect())
                        .InitializeStorageEngine()
                        .UsingJsonSerialization()
                        .Compress()
                        .UsingSynchronousDispatchScheduler(container.Resolve<IDispatchCommits>())
                        .UsingEventUpconversion().WithConvertersFromAssemblyContaining(typeof(CardCreatedUp));

                    return wireup.Build();
                }).SingleInstance();

            builder.RegisterType<InProcessDispatcher>().AsImplementedInterfaces();
            
            builder.RegisterGeneric(typeof(EventStoreRepository<>))
                .AsImplementedInterfaces();
        }
    }
}