using G12App.Domain.DomainEvents;
using G12App.Domain.Interfaces.Caching;
using G12App.Domain.Model;
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
            await _memoryCache.Remove(CacheKeys.Cell_CountTotalDiscipleshipCells);
            await _memoryCache.Remove(CacheKeys.Cell_CountTotalEvangelisticCells);
            await _memoryCache.Remove(CacheKeys.Cell_CountPendingReports);
            await _memoryCache.Remove(CacheKeys.Cell_CountWeekVisitors);
            await _memoryCache.Remove(CacheKeys.Cell_CountWeekReports);
            await _memoryCache.Remove(CacheKeys.Cell_CountWeekNewCells);
            await _memoryCache.Remove(CacheKeys.Cell_CountReportSkipped);
            await _memoryCache.Remove(CacheKeys.Cell_CountReportedCompleted);
            await _memoryCache.Remove(CacheKeys.Member_CountWithNoCellLink);
        }
    }
}
