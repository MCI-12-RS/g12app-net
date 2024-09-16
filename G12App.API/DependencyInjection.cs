using AutoMapper;
using G12App.API.Middleware;
using G12App.Domain.Mappings;
using G12App.Domain.Model;
using G12App.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace G12App.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            var jwtSection = configuration.GetSection(nameof(JWTSettings));
            services.Configure<JWTSettings>(jwtSection);
            var appSetings = jwtSection.Get<JWTSettings>();

            services.Configure<AdminUser>(
                configuration.GetSection(nameof(AdminUser)));

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

        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                return new MapperConfiguration(cfg =>
                    cfg.AddProfile(new DomainToDTOMappingProfile(httpContextAccessor))
                )
                .CreateMapper();
            });

            services.AddSingleton<ErrorMiddleware>();

            return services;
        }

        public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var mySqlServerConnString = configuration["ConnectionStrings:MySql_AppContext"];
            var sqlServerConnString = configuration["ConnectionStrings:SqlServer_AppContext"];

            if (!string.IsNullOrWhiteSpace(mySqlServerConnString))
                return
                    services.AddDbContext<AppDbContext>(options => options.UseMySql(
                        mySqlServerConnString, new MySqlServerVersion(new System.Version(5, 7))));

            if (!string.IsNullOrWhiteSpace(sqlServerConnString))
                return
                    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
                        sqlServerConnString,
                        opt => opt.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: System.TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null)));

            return services;
        }

        public static IServiceCollection RegisterCache(this IServiceCollection services)
        {
            services.AddMemoryCache();

            return services;
        }

        public static IServiceCollection RegisterWorkers(this IServiceCollection services)
        {
            // services.AddHostedService<ChildrenMemberConsolidationWorker>();
            return services;
        }

        /// <summary>
        /// Application configuration.
        /// </summary>
        /// <remarks>Used to ensure database creation and run migrations once created.</remarks>
        public static void Configure(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while applying migration. {ex.Message}");
                throw;
            }
        }
    }
}
