using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain
{
    public class EventRouter
    {
        private readonly Type _type;
        private readonly IDictionary<Type, MethodInfo> _handlers = new Dictionary<Type, MethodInfo>();

        private EventRouter(Type type)
        {
            _type = type;

            var applyMethods = _type
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m =>
                {
                    var parameters = m.GetParameters();
                    return parameters.Length == 1
                           && typeof(IEvent).IsAssignableFrom(parameters[0].ParameterType)
                           && m.ReturnParameter.ParameterType == typeof(void);
                })
                .Select(m => new
                {
                    Method = m,
                    MessageType = m.GetParameters().Single().ParameterType
                });

            foreach (var apply in applyMethods)
            {
                _handlers.Add(apply.MessageType, apply.Method);
            }
        }

        public virtual void InternalDispatch(AggregateRoot aggregate, IEvent @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            MethodInfo handler;
            if (this._handlers.TryGetValue(@event.GetType(), out handler))
                handler.Invoke(aggregate, new[] { @event });
            else
                throw new HandlerNotFoundException(@event);
        }

        static ConcurrentDictionary<Type, Lazy<EventRouter>> _routers = new ConcurrentDictionary<Type, Lazy<EventRouter>>();
        public static void Dispatch(AggregateRoot aggregate, IEvent @event)
        {
            var router = _routers.GetOrAdd(
                aggregate.GetType(),
                t => new Lazy<EventRouter>(
                    () => new EventRouter(t)))
                .Value;
            router.InternalDispatch(aggregate, @event);
        }
    }
}