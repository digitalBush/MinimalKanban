using System.Threading.Tasks;
using Autofac;
using Domain;
using Domain.Commands;

namespace Infrastructure
{
    public static class Command
    {
        static ILifetimeScope _container;
        public static void UseContainer(ILifetimeScope container)
        {
            _container = container;
        }

        public static async Task Process<T>(T cmd) where T:ICommand
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var handler = scope.Resolve<IHandleCommand<T>>();
                await handler.Handle(cmd);
            }
        }

        public static async Task<TResult> Process<TResult>(ICommand cmd)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var type = typeof (IHandleCommand<,>).MakeGenericType(cmd.GetType(), typeof (TResult));
                dynamic handler = scope.Resolve(type);
                return await handler.Handle(cmd as dynamic);
            }
        }
    }
}