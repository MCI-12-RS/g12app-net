using G12App.Domain.DomainEvents;
using G12App.Domain.Interfaces.Caching;
using Microsoft.Extensions.Logging;

namespace G12App.Application.EventHandlers.CacheInvalidation
{
    public class MemberCacheInvalidationHandler : BaseEventHandler<MemberChanged>
    {
        private readonly ICacheService _memoryCache;

        public MemberCacheInvalidationHandler(
            ICacheService memoryCache,
            ILogger<BaseEventHandler<MemberChanged>> logger)
            : base(logger)
        {
            _memoryCache = memoryCache;
        }

        protected override async Task HandleInternal(MemberChanged ev)
        {
            await _memoryCache.Remove("");
        }
    }
}
