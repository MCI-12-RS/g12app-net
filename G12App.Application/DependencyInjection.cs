using G12App.Application.Caching;
using G12App.Application.EventHandlers.CacheInvalidation;
using G12App.Application.EventHandlers.DomainEvents;
using G12App.Application.Services;
using G12App.Domain.DomainEvents;
using G12App.Domain.Interfaces.Caching;
using G12App.Domain.Interfaces.EventHandlers;
using G12App.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace G12App.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IMemberService, MemberService>();
            services.AddTransient<ICellService, CellService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IReportApp, ReportApp>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddScoped<IEntityChangeTrackingService, EntityChangeTrackingService>();
            services.AddTransient<ICacheService, MemoryCacheService>();

            services.AddTransient<IExcelService, ExcelService>();
            services.AddTransient<IColumnFormatter, ColumnFormatter>();
            services.AddTransient<IExportService, ExportService>();

            return services;
        }

        public static IServiceCollection RegisterEventHandlers(this IServiceCollection services)
        {
            services.AddTransient<IEventHandler<MemberUpdated>, MemberChangedHandler>();
            services.AddTransient<IEventHandler<MemberCreated>, MemberChangedHandler>();
            services.AddTransient<IEventHandler<MemberDeleted>, MemberChangedHandler>();
            services.AddTransient<IEventHandler<MemberDataConsolidationRequested>, MemberChangedHandler>();

            services.AddTransient<IEventHandler<CellCreated>, CellChangedHandler>();
            services.AddTransient<IEventHandler<CellUpdated>, CellChangedHandler>();
            services.AddTransient<IEventHandler<CellDeleted>, CellChangedHandler>();

            return services;
        }

        public static IServiceCollection RegisterCacheInvalidationHandlers(this IServiceCollection services)
        {
            services.AddTransient<IEventHandler<MemberUpdated>, MemberCacheInvalidationHandler>();
            services.AddTransient<IEventHandler<MemberCreated>, MemberCacheInvalidationHandler>();
            services.AddTransient<IEventHandler<MemberDeleted>, MemberCacheInvalidationHandler>();
            services.AddTransient<IEventHandler<MemberDataConsolidationRequested>, MemberCacheInvalidationHandler>();

            services.AddTransient<IEventHandler<CellCreated>, CellCacheInvalidationHandler>();
            services.AddTransient<IEventHandler<CellUpdated>, CellCacheInvalidationHandler>();
            services.AddTransient<IEventHandler<CellDeleted>, CellCacheInvalidationHandler>();

            return services;
        }
    }
}
