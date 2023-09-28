using AutoMapper;
using G12App.Domain.Mappings;
using G12App.Infrastructure.Data.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace G12App.Infrastructure.Data.Tests
{
    public class TestFixture
    {
        public AppContext Context;
        public IMapper Mapper;

        public TestFixture()
        {
            var contextOptions = new DbContextOptionsBuilder<AppContext>()
                .UseSqlite("Filename=:memory:")
                .Options;

            Context = new AppContext(contextOptions);
            Context.Database.EnsureCreated();

            Mapper = new Mapper(new MapperConfiguration(x => x.AddProfile<DomainToDTOMappingProfile>()));
        }
    }
}
