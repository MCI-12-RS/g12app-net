using AutoMapper;
using G12App.Domain.Mappings;
using G12App.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace G12App.Infrastructure.Data.Tests
{
    public class TestFixture
    {
        public AppContext Context;
        public IMapper Mapper;
        public IHttpContextAccessor HttpContextAccessor;

        public TestFixture()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var contextOptions = new DbContextOptionsBuilder<AppContext>()
                .UseSqlite(connection)
                .Options;

            Context = new AppContext(contextOptions);
            Context.Database.EnsureCreated();

            HttpContextAccessor = Mock.Of<IHttpContextAccessor>();

            Mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new DomainToDTOMappingProfile(HttpContextAccessor))
            )
            .CreateMapper();
        }

        ~TestFixture()
        {
            Context.Database.EnsureDeleted();
        }
    }
}
