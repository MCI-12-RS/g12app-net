using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasKey(e => e.Id);

            // SEED
            modelBuilder.Entity<Member>().HasData(new Member() { Id = 1, Name = "João" });
            modelBuilder.Entity<Member>().HasData(new Member() { Id = 2, Name = "Judas" });
            modelBuilder.Entity<Member>().HasData(new Member() { Id = 3, Name = "Tomé" });
            modelBuilder.Entity<Member>().HasData(new Member() { Id = 4, Name = "Felipe" });

            base.OnModelCreating(modelBuilder);
        }
    }
}
