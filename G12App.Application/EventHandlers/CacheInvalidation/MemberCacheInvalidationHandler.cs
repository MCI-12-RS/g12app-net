using G12App.Domain.DomainEvents;
using G12App.Domain.Interfaces.Caching;
using G12App.Domain.Model;
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
            await _memoryCache.Remove(CacheKeys.Member_CreatedCurrentWeek);
            await _memoryCache.Remove(CacheKeys.Member_CountActiveMembers);
            await _memoryCache.Remove(CacheKeys.Member_CountWithoutLeadership);
            await _memoryCache.Remove(CacheKeys.Member_CountWithNoCellLink);
            await _memoryCache.Remove(CacheKeys.Member_CountNonBaptized);
            await _memoryCache.Remove(CacheKeys.Member_CountIdle);
            await _memoryCache.Remove(CacheKeys.Member_CountCreatedCurrentWeek);
        }
    }
}
