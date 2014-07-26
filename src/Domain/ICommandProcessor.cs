using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.Commands;

namespace Domain
{
    public interface ICommandProcessor
    {
        Task Process<T>(T cmd) where T : ICommand;
        Task<TResult> Process<TResult>(ICommand cmd);
    }
}