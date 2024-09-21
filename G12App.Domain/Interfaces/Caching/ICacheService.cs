using System;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Caching
{
    public interface ICacheService
    {
        public Task<T> Get<T>(string key);
        Task<T> Set<T>(string key, T value);
        Task<T> GetOrFetch<T>(string key, Func<Task<T>> fetchFunction);
        Task Remove(string key);
    }
}
