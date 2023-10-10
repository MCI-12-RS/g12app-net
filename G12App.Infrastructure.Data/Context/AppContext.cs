using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace G12App.Infrastructure.Data.Context
{
    public class AppContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Report> Reports { get; set; }

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

            SeedData(builder);
        }

        private static void SeedData(ModelBuilder builder)
        {
            builder.Entity<Address>().HasData(new Address()
            {
                Id = 1,
                Street = "Biguas",
                Number = "600",
                City = "Alvorada",
                Neighborhood = "Porto Verde",
                Notes = string.Empty,
                PostalCode = "94858540",
                State = "RS",
            });

            builder.Entity<Member>().HasData(new Member()
            {
                Id = 1,
                Name = "Rafael Jorge Alves",
                BirthDate = new System.DateOnly(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
            });

            builder.Entity<Cell>().HasData(new Cell()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,

            });
        }
    }
}
