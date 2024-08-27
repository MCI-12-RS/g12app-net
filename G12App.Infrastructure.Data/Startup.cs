using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Strategies;
using G12App.Infrastructure.Data.Repositories;
using G12App.Infrastructure.Data.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace G12App.Infrastructure.Data
{
    public static class Startup
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services
                    .AddTransient<IMemberRepository, MemberRepository>()
                    .AddTransient<ICellRepository, CellRepository>()
                    .AddTransient<IReportRepository, ReportRepository>()

                    .RegisterStrategies();
        }

        public static IServiceCollection RegisterStrategies(this IServiceCollection services)
        {
            return services

                    .AddSingleton<IReportFetchListStrategy, ReportFetchListStrategy>();

        }
    }
}
