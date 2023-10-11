using AutoMapper;
using FluentAssertions;
using G12App.Application.Services;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using Moq;

namespace G12App.Application.Tests
{
    public class ReportAppTests
    {
        [Fact]
        public async void LogReportTest()
        {
            //Arrange
            var report = new ReportDTO()
            {
                ReportId = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                Members = 2,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var cell = new CellDTO()
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

            //Mock
            var mock = new Mock<ICellService>();
            mock.Setup(x => x.GetCellById(report.CellId)).ReturnsAsync(new ServiceResult<CellDTO>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDTO>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeTrue();
            log.Data.Should().NotBeNull();
        }

        [Fact]
        public async void LogReportTestShouldGetReportDateError()
        {
            //Arrange
            var report = new ReportDTO()
            {
                ReportId = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 10, 28),
                Visitors = 1,
                Members = 2,
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var cell = new CellDTO()
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

            //Mock
            var mock = new Mock<ICellService>();
            mock.Setup(x => x.GetCellById(report.CellId)).ReturnsAsync(new ServiceResult<CellDTO>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDTO>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeFalse();
            log.Errors.Should().Contain(x => "O relatório não pode ser enviado antes da célula".Equals(x.ErrorMessage));
        }

        [Fact]
        public async void LogReportShouldGetInvalidCell()
        {
            //Arrange
            var report = new ReportDTO()
            {
                ReportId = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
                Members = 2,
                CellId = 3,
                LeadershipId = 1,
                HostId = 1
            };

            var cell = new CellDTO()
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

            //Mock
            var mock = new Mock<ICellService>();
            mock.Setup(x => x.GetCellById(1)).ReturnsAsync(new ServiceResult<CellDTO>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDTO>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeFalse();
        }
    }
}
