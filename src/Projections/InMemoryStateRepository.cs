using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Denormalizers
{
    public class InMemoryStateRepository : IStateRepository
    {
        readonly IDictionary<string,object> _states=new ConcurrentDictionary<string, object>(); 

        public T Get<T>(string key)
        {
            object obj;
            if (_states.TryGetValue(key, out obj))
            {
                return (T)obj;
            }

            return default(T);
        }

        public IEnumerable<KeyValuePair<string,T>> GetList<T>(IEnumerable<string> keys)
        {
            return keys.Select(k=>new KeyValuePair<string,T>(k,Get<T>(k)));
        }

        public void Save<T>(string key, T state)
        {
            _states[key] = state;
        }
    }
}