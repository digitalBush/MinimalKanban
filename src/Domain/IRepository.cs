using System;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task<T> Get(Guid id);
        Task Save(T aggregate);
    }
}