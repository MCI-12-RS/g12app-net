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
            Mapper = new Mapper(new MapperConfiguration(x => x.AddProfile<DomainToDTOMappingProfile>()));

            HttpContextAccessor = Mock.Of<IHttpContextAccessor>();
        }

        ~TestFixture(){
            Context.Database.EnsureDeleted();
        }
    }
}
