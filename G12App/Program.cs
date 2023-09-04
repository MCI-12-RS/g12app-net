using G12App.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using G12App.Domain.Model;
using G12App.Domain.Validators;
using FluentValidation;
using System.Reflection;
using System.Collections.Generic;

namespace G12App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblies(new List<Assembly>() { typeof(Member).Assembly });
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

          

            builder.Services
                .RegisterDomain(builder.Configuration)
                .RegisterDatabase(builder.Configuration)
                .RegisterInfrastructure(builder.Configuration)
                .RegisterServices(builder.Configuration);

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Configure();
            await app.RunAsync();
        }
    }
}