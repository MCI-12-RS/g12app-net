using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace G12App.Tests
{
    public class TestsFixture
    {
        public TestsFixture()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            HttpClient = webApplicationFactory.CreateDefaultClient();

            JsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public HttpClient HttpClient { get; }
        public JsonSerializerOptions JsonSerializerOptions { get; }
    }
}
