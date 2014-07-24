using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Kanban.Controllers.events
{
    public class Aggregator : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            //DEMO HACK: Lookup user and assign groups based on access.
            return Connection.Send(connectionId,true);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            //This connection is push only
            return Task.FromResult(true);
        }
    }
}