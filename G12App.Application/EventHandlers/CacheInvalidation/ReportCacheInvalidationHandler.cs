using G12App.Application.Events;
using G12App.Domain.Interfaces.Caching;
using G12App.Domain.Model;
using Microsoft.Extensions.Logging;

namespace G12App.Application.EventHandlers.CacheInvalidation
{
    public class ReportCacheInvalidationHandler : BaseEventHandler<ReportChanged>
    {
        private readonly ICacheService _memoryCache;

        public ReportCacheInvalidationHandler(
            ICacheService memoryCache,
            ILogger<ReportCacheInvalidationHandler> logger)
            : base(logger)
        {
            _memoryCache = memoryCache;
        }

        protected override async Task HandleInternal(ReportChanged ev)
        {
            await _memoryCache.Remove(CacheKeys.Cell_CountPendingReports);
            await _memoryCache.Remove(CacheKeys.Cell_CountWeekVisitors);
            await _memoryCache.Remove(CacheKeys.Cell_CountWeekReports);
            await _memoryCache.Remove(CacheKeys.Cell_CountReportSkipped);
            await _memoryCache.Remove(CacheKeys.Cell_CountReportedCompleted);
            await _memoryCache.Remove(CacheKeys.Member_CountWithNoCellLink);
        }
    }
}