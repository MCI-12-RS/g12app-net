using G12App.API.Middleware;
using G12App.Application.Services;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Mappings;
using G12App.Domain.Model;
using G12App.Infrastructure.Data.Context;
using G12App.Infrastructure.Data.Repositories;
using G12App.Workers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace G12App.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMemberService, MemberService>();
            services.AddTransient<ICellService, CellService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IReportApp, ReportApp>();
            services.AddTransient<IAuthService, AuthService>();
            return services;
        }

        public static IServiceCollection RegisterJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            var jwtSection = configuration.GetSection("JWTSettings");
            services.Configure<JWTSettings>(jwtSection);
            var appSetings = jwtSection.Get<JWTSettings>();

            var key = Encoding.ASCII.GetBytes(appSetings.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<ICellRepository, CellRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();

            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            services.AddSingleton<ErrorMiddleware>();

            return services;
        }

        public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppContext>(options => options.UseSqlServer(
                configuration["ConnectionStrings:AppContext"],
                opt => opt.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)));

            return services;
        }

        public static IServiceCollection RegisterWorkers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<ChildrenMemberConsolidationWorker>();
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
