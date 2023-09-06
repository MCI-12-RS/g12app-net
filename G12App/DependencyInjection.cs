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

namespace G12App
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMemberService, MemberService>();
            return services;
        }

        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMemberRepository, MemberRepository>();

            services.AddScoped<IMemberService, MemberService>();
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            return services;
        }

        public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppContext>(options => options
                    .UseSqlite(configuration["ConnectionStrings:AppContext"]));

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
