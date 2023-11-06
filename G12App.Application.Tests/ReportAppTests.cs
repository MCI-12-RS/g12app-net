using FluentAssertions;
using G12App.Application.Services;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
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
                Date = DateTime.Now,
                Visitors = 1,
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
                WeekDay = DateTime.Now.DayOfWeek,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true,
                Time = TimeOnly.FromDateTime(DateTime.Now.Subtract(TimeSpan.FromMinutes(3)))
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
                Date = DateTime.Now.AddDays(1),
                Visitors = 1,
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
        public async void LogReportShouldGetInvalIdCell()
        {
            //Arrange
            var report = new ReportDTO()
            {
                ReportId = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
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
            log.Errors.Should().Contain(x => "Célula não encontrada".Equals(x.ErrorMessage));
        }

        [Fact]
        public async void LogReportShouldGetDateError()
        {
            //Arrange
            var report = new ReportDTO()
            {
                ReportId = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 10, 09),
                Visitors = 1,
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
            mock.Setup(x => x.GetCellById(1)).ReturnsAsync(new ServiceResult<CellDTO>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDTO>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeFalse();
            log.Errors.Should().Contain(x => "Relatório não pode ser lançado antes da data de cadastro da célula".Equals(x.ErrorMessage));
        }

        [Fact]
        public async void LogReportShouldGetTimeError()
        {
            //Arrange
            var report = new ReportDTO()
            {
                ReportId = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 10, 10, 10, 00, 00),
                Visitors = 1,
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
                Time = new TimeOnly(18, 00, 00),
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
            log.Errors.Should().Contain(x => "Relatório não pode ser lançado antes do horário de cadastro da célula".Equals(x.ErrorMessage));
        }

        [Fact]
        public async void LogReportShouldGetWeekError()
        {
            //Arrange
            var report = new ReportDTO()
            {
                ReportId = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = 1,
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
            mock.Setup(x => x.GetCellById(1)).ReturnsAsync(new ServiceResult<CellDTO>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDTO>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeFalse();
            log.Errors.Should().Contain(x => "O relatório precisa ser da semana atual".Equals(x.ErrorMessage));
        }
    }
}
