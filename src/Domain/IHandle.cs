using System.Threading.Tasks;
using Domain.Commands;

namespace Domain
{

    public interface IHandle<in T> where T:IEvent
    {
        void Handle(T cmd);
    }

    public interface IHandleCommand<in TRequest> where TRequest : ICommand
    {
        Task Handle(TRequest cmd);
    }

    public interface IHandleCommand<in TRequest, TResult> where TRequest : ICommand
    {
        Task<TResult> Handle(TRequest cmd);
    }
}