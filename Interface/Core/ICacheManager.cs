using System;

namespace Interface.Core
{
    public interface ICacheManager : IDisposable
    {
        T Get<T>(string Key);
        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);
        void Remove(string key);
        void Clear();

    }
}
