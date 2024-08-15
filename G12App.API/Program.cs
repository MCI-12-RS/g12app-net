using FluentValidation;
using FluentValidation.AspNetCore;
using G12App.API.Middleware;
using G12App.Application;
using G12App.Domain;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Infrastructure.Data;
using G12App.Infrastructure.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace G12App.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<Member>();

            builder.Services.AddControllersWithViews()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        return new BadRequestObjectResult(
                            new ServiceResult(
                                context.ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => new Error(x.ErrorMessage))
                            ));
                    };
                })
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                    }
                });
            });

            builder.Services
                .RegisterDomain()
                .RegisterDatabase(builder.Configuration)
                .RegisterInfrastructure()
                .RegisterRepositories()
                .RegisterServices()
                .RegisterWorkers()
                .RegisterJWT(builder.Configuration)
                .RegisterEvents()
                .AddHttpContextAccessor();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
                app.UseHsts();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(option => option.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("index.html");
            app.UseMiddleware<ErrorMiddleware>();

            app.Configure();
            await app.RunAsync();
        }
    }
}
