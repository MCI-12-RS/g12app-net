using G12App.API;
using G12App.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace G12App.Tests
{
    public class TestsFixture
    {
        public TestsFixture()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppContext>));
                            if (descriptor != null)
                                services.Remove(descriptor);

                            var conn = new SqliteConnection("Filename=:memory:");
                            conn.Open();

                            services.AddDbContext<AppContext>(options => options.UseSqlite(conn));
                        });
                    });

            HttpClient = webApplicationFactory.CreateClient();

            JsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public HttpClient HttpClient { get; }

        public JsonSerializerOptions JsonSerializerOptions { get; }
    }
}
