using Moq;
using G12App.Domain.Interfaces.Repositories;
using AutoMapper;
using G12App.Domain.Model;
using G12App.Domain.DTOs;
using G12App.Application.Services;
using FluentAssertions;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Interfaces.Events;

namespace G12App.Application.Tests
{
    public class CellServiceTests
    {
        [Fact]
        public async Task RespositoryCreateIsOk()
        {
            var cell = new Cell()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            var cellDto = new CellDTO()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            var mock = new Mock<ICellRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.Create(It.IsAny<Cell>())).ReturnsAsync(cell);
            mapperMock.Setup(x => x.Map<Cell>(cellDto)).Returns(cell);
            mapperMock.Setup(x => x.Map<CellDTO>(cell)).Returns(cellDto);

            //Act
            var service = new CellService(mock.Object, mapperMock.Object, 
                            Mock.Of<IEntityChangeTrackingService>(), Mock.Of<IEventDispatcher>());
            var result = await service.Add(cellDto);

            //Assert
            mapperMock.Verify(x => x.Map<Cell>(cellDto));
            mock.Verify(x => x.Create(cell));
            mapperMock.Verify(x => x.Map<CellDTO>(cell));
        }

        [Fact]
        public async Task RespositoryGetByIdIsOk()
        {
            var cell = new Cell()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            var cellDto = new CellDTO()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            var mock = new Mock<ICellRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetCellByIdAsync(cell.Id)).ReturnsAsync(cell);
            mapperMock.Setup(x => x.Map<CellDTO>(cell)).Returns(cellDto);

            //Act
            var service = new CellService(mock.Object, mapperMock.Object, 
                            Mock.Of<IEntityChangeTrackingService>(), Mock.Of<IEventDispatcher>());
            var result = await service.GetCellById(cell.Id);

            //Assert
            mock.Verify(x => x.GetCellByIdAsync(cell.Id));
            mapperMock.Verify(x => x.Map<CellDTO>(cell));
            result.Data.Should().Be(cellDto);
        }
        [Fact]
        public async Task RespositoryRemoveIsOk()
        {
            var cell = new Cell()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            var cellDto = new CellDTO()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            var mock = new Mock<ICellRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetCellByIdAsync(cell.Id)).ReturnsAsync(cell);
            mock.Setup(x => x.Remove(It.IsAny<Cell>())).ReturnsAsync(cell);
            mapperMock.Setup(x => x.Map<CellDTO>(cell)).Returns(cellDto);

            //Act
            var service = new CellService(mock.Object, mapperMock.Object, 
                            Mock.Of<IEntityChangeTrackingService>(), Mock.Of<IEventDispatcher>());
            var result = await service.Remove(cell.Id);

            //Assert
            mock.Verify(x => x.Remove(cell));
            
        }

        [Fact]
        public async Task RepositoryUpdateIsOk()
        {
            var cell = new Cell()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };

            var cellDto = new CellDTO()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
            };
            
            //mock
            var mock = new Mock<ICellRepository>();
            var mapperMock = new Mock<IMapper>();

            mapperMock.Setup(x => x.Map<Cell>(cellDto)).Returns(cell);
            mock.Setup(x => x.Update(It.IsAny<Cell>())).ReturnsAsync(cell);
            mapperMock.Setup(x => x.Map<CellDTO>(cell)).Returns(cellDto);

            //Act
            var service = new CellService(mock.Object, mapperMock.Object, 
                            Mock.Of<IEntityChangeTrackingService>(), Mock.Of<IEventDispatcher>());
            var result = await service.Update(cellDto, cell.Id);

            //Assert
            mapperMock.Verify(x => x.Map<Cell>(cellDto));
            mock.Verify(x => x.Update(cell));
            result.Data.Should().Be(cellDto);
        }
    }
}