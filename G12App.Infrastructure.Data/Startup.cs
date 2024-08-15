using G12App.Domain.Interfaces.Repositories;
using G12App.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace G12App.Infrastructure.Data
{
    public static class Startup
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<ICellRepository, CellRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();

            return services;
        }
    }
}
