using System.Threading.Tasks;
using Autofac;
using Domain;
using Domain.Commands;

namespace Infrastructure
{
    public class InProcessCommandProcessor : ICommandProcessor
    {
        const int RETRY_COUNT = 10;

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
                int @try=0;
                while (true)
                {
                    try
                    {
                        await handler.Handle(cmd);
                        return;
                    }
                    catch (AggregateConcurrencyException ex)
                    {
                        //TODO: Log
                        if (@try++ >= RETRY_COUNT)
                            throw;
                    }
                }
            }
        }

        public async Task<TResult> Process<TResult>(ICommand cmd)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var type = typeof(IHandleCommand<,>).MakeGenericType(cmd.GetType(), typeof(TResult));
                dynamic handler = scope.Resolve(type);
                int @try = 0;
                while (true)
                {
                    try
                    {
                        return await handler.Handle(cmd as dynamic);
                    }
                    catch (AggregateConcurrencyException ex)
                    {
                        //TODO: Log
                        if (@try++ >= RETRY_COUNT)
                            throw;
                    }
                }
            }
        }
    }
}