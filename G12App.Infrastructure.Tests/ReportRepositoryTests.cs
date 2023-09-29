using FluentAssertions;
using G12App.Domain.Model;
using G12App.Domain.Querying;
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
            HostId = 1
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
    }
}
