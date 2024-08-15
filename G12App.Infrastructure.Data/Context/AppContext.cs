using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace G12App.Infrastructure.Data.Context
{
    public class AppContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportMember> ReportMembers { get; set; }

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
            var pass1 = MD5.HashData(Encoding.UTF8.GetBytes("rafael")).Aggregate(string.Empty, (a, b) => a += b.ToString("x2"));
            var pass2 = MD5.HashData(Encoding.UTF8.GetBytes("adriano")).Aggregate(string.Empty, (a, b) => a += b.ToString("x2"));

            builder.Entity<Member>().HasData(new Member()
            {
                Id = 1,
                Name = "Samuel Colares",
                Gender = Domain.Model.Enum.Gender.Male,
                MinistryLevel = 1
            });

            builder.Entity<Member>().HasData(new Member()
            {
                Id = 2,
                Name = "Eduardo Coelho",
                Gender = Domain.Model.Enum.Gender.Male,
                LeadershipId = 1,
                MinistryLevel = 12,
            });

            builder.Entity<Member>().HasData(new Member()
            {
                Id = 3,
                Name = "Anderson Pereira",
                LeadershipId = 1,
                MinistryLevel = 12,
            });

            builder.Entity<Member>().HasData(new Member()
            {
                Id = 4,
                Name = "Rafael Alves",
                Email = "rafaeljorgealves@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                Password = pass1,
                LeadershipId = 2,
                MinistryLevel = 144,
            });

            builder.Entity<Member>().HasData(new Member()
            {
                Id = 5,
                Name = "Adriano Ramos",
                Email = "adrircorrea123@gmail.com",
                Gender = Domain.Model.Enum.Gender.Male,
                Password = pass2,
                LeadershipId = 3,
                MinistryLevel = 144,
            });

            builder.Entity<Cell>().HasData(new Cell
            {
                Id = 1,
                LeadId = 4,
                HostId = 4,
                TimothyId = 4,
                WeekDay = System.DayOfWeek.Monday,
                Time = new System.TimeOnly(20, 30).ToString(),
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            });

            builder.Entity<Cell>().HasData(new Cell
            {
                Id = 2,
                LeadId = 5,
                HostId = 5,
                TimothyId = 5,
                WeekDay = System.DayOfWeek.Saturday,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            });
        }
    }
}
