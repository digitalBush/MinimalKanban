using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Features.Variance;
using Autofac.Integration.WebApi;
using Denormalizers;
using Domain;
using Infrastructure;
using Infrastructure.Persistence;
using Kanban.Helpers;
using Kanban.Services;

namespace Kanban
{
    public class Bootstrapper
    {
        public static void Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyTypes(typeof (IHandle<>).Assembly)
                .AsImplementedInterfaces();


            builder.RegisterAssemblyTypes(typeof(Denormalizer<>).Assembly)
                .AsImplementedInterfaces();

            //HACK
            builder.RegisterInstance(new Replay.Runner().Rebuild())
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterModule<EventStoreModule>();
            builder.RegisterType<SignalREventDispatcher>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<InProcessCommandProcessor>()
                .AsImplementedInterfaces()
                .SingleInstance();
            
            builder.RegisterSource(new ContravariantRegistrationSource());

            var container = builder.Build();

            Command.UseContainer(container.BeginLifetimeScope());
            Denormalizer.UseContainer(container.BeginLifetimeScope());
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}