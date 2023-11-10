using FluentValidation;
using FluentValidation.AspNetCore;
using G12App.Domain;
using G12App.Domain.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using G12App.API.Middleware;

namespace G12App.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters().AddValidatorsFromAssemblyContaining<Member>();
            builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .RegisterDomain(builder.Configuration)
                .RegisterDatabase(builder.Configuration)
                .RegisterInfrastructure(builder.Configuration)
                .RegisterServices(builder.Configuration)
                .RegisterJWT(builder.Configuration);

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("index.html");
            app.UseMiddleware<ErrorMiddleware>();

            app.Configure();
            await app.RunAsync();
        }
    }
}
