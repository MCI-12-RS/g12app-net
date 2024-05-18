using G12App.Application.Services;
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
            return services;
        }

    }
}
