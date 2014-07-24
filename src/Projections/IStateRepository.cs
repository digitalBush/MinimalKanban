using System.Collections.Generic;

namespace Denormalizers
{
    public interface IStateRepository
    {
        T Get<T>(string key);
        IEnumerable<KeyValuePair<string,T>> GetList<T>(IEnumerable<string> keys);
        void Save<T>(string key, T state);
    }
}