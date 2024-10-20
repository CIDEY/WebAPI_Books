using BooksAPI.Service.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace BooksAPI.Service
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public T GetOrCreate<T>(string key, Func<T> createItem, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            if (!_memoryCache.TryGetValue(key, out T cacheEntry))
            {
                cacheEntry = createItem();

                var cacheEnrtyOptions = new MemoryCacheEntryOptions();

                if (absoluteExpireTime.HasValue)
                    cacheEnrtyOptions.SetAbsoluteExpiration(absoluteExpireTime.Value);

                if (unusedExpireTime.HasValue)
                    cacheEnrtyOptions.SetSlidingExpiration(unusedExpireTime.Value);

                _memoryCache.Set(key, cacheEntry, cacheEnrtyOptions);
            }

            return cacheEntry;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
