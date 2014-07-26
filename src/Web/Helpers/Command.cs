using System.Threading.Tasks;
using Autofac;
using Domain.Commands;
using Infrastructure;

namespace Kanban.Helpers
{
    public static class Command
    {
        static InProcessCommandProcessor _processor;
        public static void UseContainer(ILifetimeScope container)
        {
            _processor = new InProcessCommandProcessor(container);
        }

        public static Task Process<T>(T cmd) where T : ICommand
        {
            return _processor.Process(cmd);
        }

        public static Task<TResult> Process<TResult>(ICommand cmd)
        {
            return _processor.Process<TResult>(cmd);
        }
    }
}