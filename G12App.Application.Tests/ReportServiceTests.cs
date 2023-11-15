using AutoMapper;
using FluentAssertions;
using G12App.Application.Services;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using Moq;

namespace G12App.Application.Tests
{
    public class ReportServiceTests
    {
        [Fact]
        public async void AddReportTest()
        {
            //Arrange
            var report = new Report()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var reportDto = new ReportDTO()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            //Mock
            var mock = new Mock<IReportRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.Create(It.IsAny<Report>())).ReturnsAsync(report);
            mapperMock.Setup(x => x.Map<Report>(reportDto)).Returns(report);
            mapperMock.Setup(x => x.Map<ReportDTO>(report)).Returns(reportDto);

            //Act
            var service = new ReportService(mock.Object, mapperMock.Object, Mock.Of<IEntityChangeTrackingService>());
            var result = await service.Add(reportDto);

            //Arrange
            mapperMock.Verify(x => x.Map<Report>(reportDto));
            mock.Verify(x => x.Create(report));
            mapperMock.Verify(x => x.Map<ReportDTO>(report));
        }

        [Fact]
        public async void GetReportById()
        {
            //Arrange
            var report = new Report()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var reportDto = new ReportDTO()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            //Mock
            var mock = new Mock<IReportRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetReportById(report.Id)).ReturnsAsync(report);
            mapperMock.Setup(x => x.Map<ReportDTO>(report)).Returns(reportDto);

            //Act
            var service = new ReportService(mock.Object, mapperMock.Object, Mock.Of<IEntityChangeTrackingService>());
            var result = await service.GetById(report.Id);

            //Assert
            mock.Verify(x => x.GetReportById(report.Id));
            mapperMock.Verify(x => x.Map<ReportDTO>(report));
            result.Data.Should().Be(reportDto);
        }

        [Fact]
        public async void RemoveTest()
        {
            //Arrange
            var report = new Report()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var reportDto = new ReportDTO()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            //Mock
            var mock = new Mock<IReportRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetReportById(report.Id)).ReturnsAsync(report);
            mock.Setup(x => x.Remove(It.IsAny<Report>())).ReturnsAsync(report);
            mapperMock.Setup(x => x.Map<ReportDTO>(report)).Returns(reportDto);

            //Act
            var service = new ReportService(mock.Object, mapperMock.Object, Mock.Of<IEntityChangeTrackingService>());
            var result = await service.Delete(report.Id);

            //Assert
            mock.Verify(x => x.Remove(report));
        }
    }
}
