using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace G12App.Infrastructure.Data.Context
{
    public class AppContext : DbContext
    {
        public DbSet<Member> Members { get; set; }

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
        }

        internal Task<Member> FindAsync(int? id)
        {
            throw new System.NotImplementedException();
        }
    }
}
