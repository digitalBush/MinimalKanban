using System.Threading.Tasks;
using Autofac;
using Domain;
using Domain.Commands;

namespace Infrastructure
{
    public class InProcessCommandProcessor : ICommandProcessor
    {
        readonly ILifetimeScope _container;

        public InProcessCommandProcessor(ILifetimeScope container)
        {
            _container = container;
        }

        public async Task Process<T>(T cmd) where T : ICommand
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var handler = scope.Resolve<IHandleCommand<T>>();
                await handler.Handle(cmd);
            }
        }

        public async Task<TResult> Process<TResult>(ICommand cmd)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var type = typeof(IHandleCommand<,>).MakeGenericType(cmd.GetType(), typeof(TResult));
                dynamic handler = scope.Resolve(type);
                return await handler.Handle(cmd as dynamic);
            }
        }
    }
}