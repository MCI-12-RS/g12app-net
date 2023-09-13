using AutoMapper;
using FluentAssertions;
using G12App.Domain.Mappings;
using G12App.Domain.Model;
using G12App.Infrastructure.Data.Repositories;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace G12App.Infrastructure.Data.Tests
{
    public class CellRepositoryTests
    {
        private readonly TestFixture _fixture = new TestFixture();
        private readonly IMapper _mapper;
        private readonly CellRepository _repository;

        public CellRepositoryTests()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile<DomainToDTOMappingProfile>()));
            _repository = new CellRepository(_fixture.Context, _mapper);
        }

        [Fact]
        public async Task TestUpdate()
        {
            // Arrange
            var cellToCreate = new Cell()
            {
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

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

            var createdCell = await _repository.Create(cellToCreate);

            // Act
            cellToUpdate.Id = createdCell.Id;
            var updatedCell = await _repository.Update(cellToUpdate);

            // Assert
            var getCell = await _repository.GetCellByIdAsync(createdCell.Id);
            getCell.Should().NotBeNull();
            getCell.CellType.Should().Be(Domain.Model.Enum.CellType.Discipleship);
        }
    }
}