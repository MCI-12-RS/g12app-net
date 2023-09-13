using G12App.Infrastructure.Data.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace G12App.Infrastructure.Data.Tests
{
    public class TestFixture
    {
        public AppContext Context;

        public TestFixture()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var contextOptions = new DbContextOptionsBuilder<AppContext>()
                .UseSqlite(connection)
                .Options;

            Context = new AppContext(contextOptions);
            Context.Database.EnsureCreated();
        }
    }
}
