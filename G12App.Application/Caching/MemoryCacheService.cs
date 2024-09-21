using G12App.Domain.Interfaces.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace G12App.Application.Caching
{
    public class MemoryCacheService : ICacheService
    {
        private IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> Get<T>(string key)
        {
            await Task.Yield();
            return _memoryCache.Get<T>(key);
        }

        public async Task<T> Set<T>(string key, T value)
        {
            await Task.Yield();
            return _memoryCache.Set(key, value);
        }

        public async Task<T> GetOrFetch<T>(string key, Func<Task<T>> fetchFunction)
        {
            return await _memoryCache.GetOrCreateAsync(key, (cacheEntry) => fetchFunction());
        }

        public async Task Remove(string key)
        {
            await Task.Yield();
            _memoryCache.Remove(key);
        }
    }
}
