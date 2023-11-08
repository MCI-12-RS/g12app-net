using G12App.Application.Services;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Mappings;
using G12App.Infrastructure.Data.Context;
using G12App.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace G12App.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMemberService, MemberService>();
            services.AddTransient<ICellService, CellService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReportApp, ReportApp>();
            return services;
        }

        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<ICellRepository, CellRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            return services;
        }

        public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppContext>(options => options.UseMySql(
                configuration["ConnectionStrings:AppContext"], ServerVersion.AutoDetect(configuration["ConnectionStrings:AppContext"])));

            return services;
        }

        /// <summary>
        /// Application configuration.
        /// </summary>
        /// <remarks>Used to ensure database creation and run migrations once created.</remarks>
        public static void Configure(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppContext>();
            context.Database.EnsureCreated();
        }
    }
}
