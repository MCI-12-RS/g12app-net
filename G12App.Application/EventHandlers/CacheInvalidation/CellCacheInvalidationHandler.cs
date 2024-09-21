using G12App.Domain.DomainEvents;
using G12App.Domain.Interfaces.Caching;
using Microsoft.Extensions.Logging;

namespace G12App.Application.EventHandlers.CacheInvalidation
{
    public class CellCacheInvalidationHandler : BaseEventHandler<CellChanged>
    {
        private readonly ICacheService _memoryCache;

        public CellCacheInvalidationHandler(
            ICacheService memoryCache,
            ILogger<BaseEventHandler<CellChanged>> logger)
            : base(logger)
        {
            _memoryCache = memoryCache;
        }

        protected override async Task HandleInternal(CellChanged ev)
        {
            await _memoryCache.Remove("");
        }
    }
}
