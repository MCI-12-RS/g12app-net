using AutoMapper;
using FluentAssertions;
using G12App.Application.Services;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Events;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using Moq;

namespace G12App.Application.Tests
{
    public class ReportServiceTests
    {
        [Fact]
        public async Task AddReportTest()
        {
            //Arrange
            var report = new Report()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = "a",
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var reportDto = new ReportDto()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = ["a"],
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            //Mock
            var mock = new Mock<IReportRepository>();
            var mockMemberRepo = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.Create(It.IsAny<Report>())).ReturnsAsync(report);
            mapperMock.Setup(x => x.Map<Report>(reportDto)).Returns(report);
            mapperMock.Setup(x => x.Map<ReportDto>(report)).Returns(reportDto);

            //Act
            var service = new ReportService(mock.Object, mockMemberRepo.Object, mapperMock.Object, 
                Mock.Of<IEntityChangeTrackingService>(), Mock.Of<IEventDispatcher>());
            var result = await service.Add(reportDto);

            //Arrange
            mapperMock.Verify(x => x.Map<Report>(reportDto));
            mock.Verify(x => x.Create(report));
            mapperMock.Verify(x => x.Map<ReportDto>(report));
        }

        [Fact]
        public async Task GetReportById()
        {
            //Arrange
            var report = new Report()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = "a",
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var reportDto = new ReportDto()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = ["a"],
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            //Mock
            var mock = new Mock<IReportRepository>();
            var mockMemberRepo = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetReportById(report.Id)).ReturnsAsync(report);
            mapperMock.Setup(x => x.Map<ReportDto>(report)).Returns(reportDto);

            //Act
            var service = new ReportService(mock.Object, mockMemberRepo.Object, mapperMock.Object, 
                            Mock.Of<IEntityChangeTrackingService>(), Mock.Of<IEventDispatcher>());
            var result = await service.GetById(report.Id);

            //Assert
            mock.Verify(x => x.GetReportById(report.Id));
            mapperMock.Verify(x => x.Map<ReportDto>(report));
            result.Data.Should().Be(reportDto);
        }

        [Fact]
        public async Task RemoveTest()
        {
            //Arrange
            var report = new Report()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = "string",
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var reportDto = new ReportDto()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = ["a"],
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            //Mock
            var mock = new Mock<IReportRepository>();
            var mockMemberRepo = new Mock<IMemberRepository>();
            var mapperMock = new Mock<IMapper>();

            mock.Setup(x => x.GetReportById(report.Id)).ReturnsAsync(report);
            mock.Setup(x => x.Remove(It.IsAny<Report>())).ReturnsAsync(report);
            mapperMock.Setup(x => x.Map<ReportDto>(report)).Returns(reportDto);

            //Act
            var service = new ReportService(mock.Object, mockMemberRepo.Object, mapperMock.Object, 
                            Mock.Of<IEntityChangeTrackingService>(), Mock.Of<IEventDispatcher>());
            var result = await service.Delete(report.Id);

            //Assert
            mock.Verify(x => x.Remove(report));
        }
    }
}
