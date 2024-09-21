using G12App.Domain.Caching;
using System;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Caching
{
    public interface ICacheService
    {
        public Task<T> Get<T>(CacheSettings settings);
        Task<T> Set<T>(CacheSettings settings, T value);
        Task<T> GetOrFetch<T>(CacheSettings settings, Func<Task<T>> fetchFunction);
        Task Remove(CacheSettings settings);
    }
}
