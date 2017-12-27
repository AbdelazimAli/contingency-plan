using System;
using Interface.Core;
using System.Runtime.Caching;

namespace Db.Persistence.Services
{
    class CacheManager : ICacheManager
    {
        public CacheManager()
        {

        }

        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public virtual void Dispose()
        {

        }

        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }

        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = (cacheTime != 0 ? DateTime.Now + TimeSpan.FromMinutes(cacheTime) : DateTime.MaxValue);
            Cache.Add(new CacheItem(key, data), policy);
        }
    }
}
