using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Caching
{
    public interface ICachingService
    {
        T Get<T>(string key);

        bool TryGet<T>(string key, out T val);

        Task<T> GetOrSetCache<T>(string key, Func<Task<T>> buildData, int minutes = 2);

        void Add<T>(T o, string key);

        void Remove(string key);
    }

    public class CachingService : ICachingService
    {
        private IMemoryCache _cache;

        public CachingService()
        {

        }

        public CachingService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public bool TryGet<T>(string key, out T val)
        {
            return _cache.TryGetValue<T>(key, out val);
        }

        public async Task<T> GetOrSetCache<T>(string key, Func<Task<T>> buildDataCache, int minutes = 2)
        {
            if (_cache.TryGetValue<T>(key, out T data) && data != null)
                return data;

            data = await buildDataCache();
            _cache.Set(key, data, DateTimeOffset.Now.AddMinutes(minutes));

            return data;
        }

        public void Add<T>(T c, string key)
        {
            T entryCache;

            if (!_cache.TryGetValue(key, out entryCache))
            {
                entryCache = c;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(300)); // 2p

                _cache.Set(key, entryCache, cacheEntryOptions);
            }
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
