using G12App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace G12App.Infrastructure.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportMember> ReportMembers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (Debugger.IsAttached)
                optionsBuilder.LogTo((query) =>
                {
                    if ((query?.Contains("Executing DbCommand")).GetValueOrDefault())
                        Console.WriteLine("Query: " + query);
                });
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
