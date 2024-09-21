using G12App.Domain.Caching;
using G12App.Domain.Interfaces.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace G12App.Application.Caching
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> Get<T>(CacheSettings settings)
        {
            await Task.Yield();
            return _memoryCache.Get<T>(settings.Key);
        }

        public async Task<T> Set<T>(CacheSettings settings, T value)
        {
            await Task.Yield();
            return _memoryCache.Set(settings.Key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = settings.ExpiresIn,
            });
        }

        public async Task<T> GetOrFetch<T>(CacheSettings settings, Func<Task<T>> fetchFunction)
        {
            return await _memoryCache.GetOrCreateAsync(settings.Key, async (cacheEntry) =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = settings.ExpiresIn;
                return await fetchFunction();
            });
        }

        public async Task Remove(CacheSettings settings)
        {
            await Task.Yield();
            _memoryCache.Remove(settings.Key);
        }
    }
}
