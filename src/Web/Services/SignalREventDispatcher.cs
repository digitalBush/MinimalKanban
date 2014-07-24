using Domain;
using Kanban.Controllers.events;
using Microsoft.AspNet.SignalR;

namespace Kanban.Services
{
    public class SignalREventDispatcher:IUIEventDispatcher
    {
        readonly IPersistentConnectionContext _context;

        public SignalREventDispatcher()
        {
            _context = GlobalHost.ConnectionManager.GetConnectionContext<Aggregator>();
        }

        public void Notify(string viewName, object o)
        {
            _context.Connection.Broadcast(new {
                name = viewName,
                data = o
            });
        }

        public void Notify(string viewName, string key, object o)
        {
            _context.Connection.Broadcast(new
            {
                name = viewName + ":" + key,
                data = o
            });
        }
    }
}