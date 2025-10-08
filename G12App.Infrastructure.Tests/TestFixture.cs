using AutoMapper;
using G12App.Domain.Mappings;
using G12App.Domain.Model;
using G12App.Domain.Model.Enum;
using G12App.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace G12App.Infrastructure.Data.Tests
{
    public class TestFixture
    {
        public AppDbContext Context;
        public IMapper Mapper;
        public IHttpContextAccessor HttpContextAccessor;

        public TestFixture()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging().LogTo(Console.WriteLine)
                .Options;

            Context = new AppDbContext(contextOptions);
            Context.Database.EnsureCreated();

            var address = new Address
            {
                Street = "Test Street",
                Number = "123",
                Neighborhood = "Test Neighborhood",
                City = "Test City",
                State = "TS",
                PostalCode = "00000"
            };

            var member = new Member
            {
                Name = "Seed Member",
                Email = "seed.member@example.com",
                Gender = Gender.Male,
                HomeAddress = address
            };

            var memberTwo = new Member
            {
                Name = "Seed Member Two",
                Email = "seed.member.two@example.com",
                Gender = Gender.Female,
                HomeAddress = address
            };

            Context.Addresses.Add(address);
            Context.Members.AddRange(member, memberTwo);
            Context.SaveChanges();

            var cell = new Cell
            {
                LeadId = member.Id,
                HostId = member.Id,
                TimothyId = member.Id,
                WeekDay = DayOfWeek.Monday,
                Time = "19:00",
                PlaceId = address.Id,
                CellType = CellType.Discipleship,
                ReportMandatory = true
            };

            Context.Cells.Add(cell);
            Context.SaveChanges();
            Context.ChangeTracker.Clear();

            

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
