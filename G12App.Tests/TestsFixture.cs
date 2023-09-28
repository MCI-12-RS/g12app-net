using G12App.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace G12App.Tests
{
    public class TestsFixture
    {
        public TestsFixture()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder
                    .ConfigureTestServices(services =>
                    {
                        var connection = new SqliteConnection("Filename=:memory:");
                        connection.Open();

                        services.AddDbContext<AppContext>(options => options.UseSqlite(connection));
                    });
            });

            HttpClient = webApplicationFactory.CreateDefaultClient();

            JsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public HttpClient HttpClient { get; }

        public JsonSerializerOptions JsonSerializerOptions { get; }
    }
}
