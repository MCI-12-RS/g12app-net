using AutoMapper;
using FluentAssertions;
using G12App.Domain.Mappings;
using G12App.Domain.Model;
using G12App.Domain.Model.Enum;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using G12App.Infrastructure.Data.Repositories;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace G12App.Infrastructure.Data.Tests
{
    public class CellRepositoryTests
    {
        private readonly TestFixture _fixture = new TestFixture();
        private readonly CellRepository _repository;
        private readonly Cell _defaultCellToCreate = new Cell()
        {
            LeadId = 1,
            HostId = 1,
            TimothyId = 1,
            WeekDay = System.DayOfWeek.Tuesday,
            PlaceId = 1,
            CellType = Domain.Model.Enum.CellType.Evangelistic,
            ReportMandatory = true,
        };

        public CellRepositoryTests()
        {

            _repository = new CellRepository(_fixture.Context, _fixture.HttpContextAccessor);
        }

        [Fact]
        public async Task TestUpdate()
        {
            // Arrange


            var cellToUpdate = new Cell()
            {
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Discipleship,
                ReportMandatory = true,
            };

            var createdCell = await _repository.Create(_defaultCellToCreate);

            // Act
            cellToUpdate.Id = createdCell.Id;
            var updatedCell = await _repository.Update(cellToUpdate);

            // Assert
            var getCell = await _repository.GetCellByIdAsync(createdCell.Id);
            getCell.Should().NotBeNull();
            getCell.CellType.Should().Be(Domain.Model.Enum.CellType.Discipleship);
        }

        [Fact]
        public async Task TestRemove()
        {
            //Arrange
            var createdCell = await _repository.Create(_defaultCellToCreate);

            //Act
            var result = await _repository.Remove(createdCell);

            //Assert
            result.IsDeleted.Should().BeTrue();
        }
        [Fact]
        public async Task TestToCreate()
        {

            //Act
            var result = await _repository.Create(_defaultCellToCreate);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdTest()
        {
            //Arrange
            var createdCell = await _repository.Create(_defaultCellToCreate);

            //Act
            var result = await _repository.GetCellByIdAsync(createdCell.Id);

            //Assert
            result.Should().Be(createdCell);
        }

        [Fact]
        public async Task GetCellTest()
        {
            //Arrange
            var createdCell = await _repository.Create(_defaultCellToCreate);
            var query = new Query<CellFilter>();

            //Act
            var result = await _repository.GetCellsAsync(query);

            //Assert
            result.Should().NotBeNull();
        }
    }
}