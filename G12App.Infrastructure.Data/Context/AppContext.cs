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
        public DbSet<User> Users { get; set; }

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

            builder.Entity<Address>().HasData(new Address()
            {
                Id = 2,
                Street = "Oscar Schick",
                Number = "0",
                City = "Alvorada",
                Neighborhood = "Formosa",
                Notes = string.Empty,
                PostalCode = "00000000",
                State = "RS",
            });

            builder.Entity<Address>().HasData(new Address()
            {
                Id = 3,
                Street = "Rua da Figueira",
                Number = "0",
                City = "Alvorada",
                Neighborhood = "Sumare",
                Notes = string.Empty,
                PostalCode = "00000000",
                State = "RS",
            });

            builder.Entity<Member>().HasData(new Member()
            {
                Id = 1,
                Name = "Samuel Colares",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "aaaaa@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 2,
                MinistryLevel = 0
            });

            builder.Entity<Member>().HasData(new Member()
            {
                Id = 2,
                Name = "Eduardo Coelho",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "bbbbbb@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 3,
                LeadershipId = 1,
                MinistryLevel = 1,
            });

            builder.Entity<Member>().HasData(new Member()
            {
                Id = 3,
                Name = "Rafael Alves",
                BirthDate = new System.DateTime(1993, 05, 10),
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                HomeAddressId = 1,
                LeadershipId = 2,
                MinistryLevel = 2,
            });

            builder.Entity<Cell>().HasData(new Cell()
            {
                Id = 1,
                LeadId = 3,
                HostId = 3,
                TimothyId = 2,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                Time = new System.TimeOnly(20, 00).ToString(),
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            });

            builder.Entity<Cell>().HasData(new Cell()
            {
                Id = 2,
                LeadId = 2,
                HostId = 3,
                TimothyId = 2,
                WeekDay = System.DayOfWeek.Wednesday,
                PlaceId = 3,
                CellType = Domain.Model.Enum.CellType.Discipleship,
                ReportMandatory = true,
            });
        }
    }
}
