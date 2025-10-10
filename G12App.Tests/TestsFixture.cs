using G12App.API;
using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace G12App.Tests
{
    public class TestsFixture
    {
        private const string IntegrationTestSecret = "integration-test-secret-key-which-is-long-enough";

        public TestsFixture()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                            if (descriptor != null)
                                services.Remove(descriptor);

                            var conn = new SqliteConnection("Filename=:memory:");
                            conn.Open();

                            services.AddDbContext<AppDbContext>(options => options
                                .EnableSensitiveDataLogging().LogTo(System.Console.WriteLine)
                                .UseSqlite(conn));

                            using var sp = services.BuildServiceProvider();
                            using var scope = sp.CreateScope();
                            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            context.Database.EnsureCreated();

                            SeedDatabase(context);

                            services.RemoveAll<IOptions<JWTSettings>>();
                            services.Configure<JWTSettings>(options =>
                            {
                                options.SecretKey = IntegrationTestSecret;
                            });

                            services.RemoveAll<IOptions<AdminUser>>();
                            services.Configure<AdminUser>(options =>
                            {
                                options.UserName = "testuser";
                                options.Password = "testpassword";
                            });

                            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                            {
                                var key = Encoding.ASCII.GetBytes(IntegrationTestSecret);
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(key),
                                    ValidateIssuer = false,
                                    ValidateAudience = false
                                };
                            });
                        });
                    });

            HttpClient = webApplicationFactory.CreateClient();

            JsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            AuthenticateClientAsync().GetAwaiter().GetResult();
        }

        public HttpClient HttpClient { get; }

        public JsonSerializerOptions JsonSerializerOptions { get; }

        private async Task AuthenticateClientAsync()
        {
            var response = await HttpClient.PostAsJsonAsync("/api/auth/login", new
            {
                UserName = "testuser",
                Password = "testpassword"
            });

            response.EnsureSuccessStatusCode();
            var loginResponse = await response.Content.ReadFromJsonAsync<ServiceResult<ResponseLoginDto>>(JsonSerializerOptions);
            if (loginResponse?.Data?.Token is null)
                throw new InvalidOperationException("Failed to obtain auth token for integration tests.");

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Data.Token);
        }

        private static void SeedDatabase(AppDbContext context)
        {
            if (context.Members.Any())
                return;

            var address = new Address
            {
                Street = "Integration Street",
                Number = "100",
                Neighborhood = "Integration Neighborhood",
                City = "Integration City",
                State = "IC",
                PostalCode = "12345"
            };

            var leadMember = new Member
            {
                Name = "Integration Lead",
                Email = "lead@example.com",
                Gender = Domain.Model.Enum.Gender.Male,
                Role = Roles.AppSuperAdmin,
                Password = "testpassword",
                HomeAddress = address
            };

            var hostMember = new Member
            {
                Name = "Integration Host",
                Email = "host@example.com",
                Gender = Domain.Model.Enum.Gender.Female,
                Role = Roles.AppUser,
                HomeAddress = address
            };

            var timothyMember = new Member
            {
                Name = "Integration Timothy",
                Email = "timothy@example.com",
                Gender = Domain.Model.Enum.Gender.Male,
                Role = Roles.AppUser,
                HomeAddress = address
            };

            context.Addresses.Add(address);
            context.Members.AddRange(leadMember, hostMember, timothyMember);
            context.SaveChanges();

            var cell = new Cell
            {
                LeadId = leadMember.Id,
                HostId = hostMember.Id,
                TimothyId = timothyMember.Id,
                WeekDay = DayOfWeek.Wednesday,
                Time = "20:00",
                PlaceId = address.Id,
                CellType = Domain.Model.Enum.CellType.Discipleship,
                ReportMandatory = true
            };

            context.Cells.Add(cell);

            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
