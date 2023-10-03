using FluentAssertions;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using G12App.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace G12App.Infrastructure.Data.Tests
{
    public class ReportRepositoryTests
    {
        private readonly TestFixture _fixture = new TestFixture();
        private readonly ReportRepository _repository;
        private readonly Report _defaultReport = new Report()
        {
            ReportId = 1,
            Subject = "Valid subject",
            Date = new DateTime(2023, 9, 28),
            Visitors = 1,
            Members = 1,
            CellId = 1,
            LeadershipId = 1,
            HostId = 1,
            CellType = Domain.Model.Enum.CellType.Discipleship
        };
        private readonly Query query = new Query();

        public ReportRepositoryTests()
        {
            _repository = new ReportRepository(_fixture.Context, _fixture.Mapper);
        }

        [Fact]
        public async Task CreateTest()
        {
            //Arrange
            var createdReport = new Report()
            {
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                Members = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            //Act
            var result = await _repository.Create(createdReport);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetReportsTest()
        {
            //Arrange
            var dfReport = _defaultReport;
            var query = new Query<ReportFilter>();

            //Act
            var result = await _repository.GetReportsAsync(query);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetReportByIdTest()
        {
            //Arrange
            var toCreateReport = new Report()
            {
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                Members = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };
            var createdReport = await _repository.Create(toCreateReport);

            //Act
            var result = await _repository.GetReportById(createdReport.ReportId);

            //Assert
            result.Should().Be(createdReport);
        }

        [Fact]
        public async Task RemoveTest()
        {
            //Arrange
            var toCreateReport = new Report()
            {
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                Members = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };
            var createdReport = await _repository.Create(toCreateReport);

            //Act
            var result = await _repository.Remove(createdReport);

            //Assert
            result.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateTest()
        {
            //Arrange
            var toCreateReport = new Report()
            {
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                Members = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var toUpdateReport = new Report()
            {
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                Members = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 2
            };
            var createdReport = await _repository.Create(toCreateReport);

            //Act
            var result = await _repository.Update(toUpdateReport);

            //Assert
            result.Should().Be(toUpdateReport);

        }
    }
}
