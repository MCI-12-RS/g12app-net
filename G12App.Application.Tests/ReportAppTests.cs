using FluentAssertions;
using G12App.Application.Services;
using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using Moq;

namespace G12App.Application.Tests
{
    public class ReportAppTests
    {
        [Fact(Skip = "Test is unstable depending on the time it is run")]
        public async Task LogReportTest()
        {
            //Arrange
            var report = new ReportDto()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = DateTime.Now,
                Visitors = new List<string> { "a" },
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var cell = new CellDto()
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
            mock.Setup(x => x.GetCellById(report.CellId)).ReturnsAsync(new ServiceResult<CellDto>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDto>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeTrue();
            log.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task LogReportTestShouldGetReportDateError()
        {
            //Arrange
            var report = new ReportDto()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = DateTime.Now.AddDays(1),
                Visitors = ["a"],
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var cell = new CellDto()
            {
                Id = 1,
                LeadId = 1,
                HostId = 1,
                TimothyId = 1,
                WeekDay = System.DayOfWeek.Tuesday,
                PlaceId = 1,
                CellType = Domain.Model.Enum.CellType.Evangelistic,
                ReportMandatory = true
            };

            //Mock
            var mock = new Mock<ICellService>();
            mock.Setup(x => x.GetCellById(report.CellId)).ReturnsAsync(new ServiceResult<CellDto>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDto>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeFalse();
            log.Errors.Should().Contain(x => "O relatório não pode ser enviado para uma data futura.".Equals(x.ErrorMessage));
        }

        [Fact]
        public async Task LogReportShouldGetInvalidCell()
        {
            //Arrange
            var report = new ReportDto()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = SystemConstants.BrazilTime.DateTime.GetStartOfWeek().AddDays(2),
                Visitors = ["a"],
                CellId = 3,
                LeadershipId = 1,
                HostId = 1
            };

            var cell = new CellDto()
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
            mock.Setup(x => x.GetCellById(1)).ReturnsAsync(new ServiceResult<CellDto>(null));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDto>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeFalse();
            log.Errors.Should().Contain(x => "Célula não encontrada.".Equals(x.ErrorMessage));
        }

        [Fact]
        public async Task LogReportShouldGetDateError()
        {
            //Arrange
            var report = new ReportDto()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = SystemConstants.BrazilTime.DateTime.GetStartOfWeek().AddDays(1),
                Visitors = ["a"],
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var cell = new CellDto()
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
            mock.Setup(x => x.GetCellById(1)).ReturnsAsync(new ServiceResult<CellDto>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDto>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeFalse();
            log.Errors.Should().Contain(x => "O relatório não pode ser lançado antes da data de cadastro da célula.".Equals(x.ErrorMessage));
        }

        [Fact]
        public async Task LogReportShouldGetTimeError()
        {
            //Arrange
            var report = new ReportDto()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = SystemConstants.BrazilTime.DateTime.GetStartOfWeek().AddDays(2).AddHours(8),
                Visitors = ["a"],
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var cell = new CellDto()
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
            mock.Setup(x => x.GetCellById(1)).ReturnsAsync(new ServiceResult<CellDto>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDto>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeFalse();
            log.Errors.Should().Contain(x => "O relatório não pode ser lançado antes do horário de cadastro da célula (18:00).".Equals(x.ErrorMessage));
        }

        [Fact]
        public async Task LogReportShouldGetWeekError()
        {
            //Arrange
            var report = new ReportDto()
            {
                Id = 1,
                Subject = "Valid subject",
                Date = new DateTime(2023, 9, 28),
                Visitors = ["a"],
                CellId = 1,
                LeadershipId = 1,
                HostId = 1
            };

            var cell = new CellDto()
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
            mock.Setup(x => x.GetCellById(1)).ReturnsAsync(new ServiceResult<CellDto>(cell));
            var mockReportService = new Mock<IReportService>();
            mockReportService.Setup(x => x.Add(report)).ReturnsAsync(new ServiceResult<ReportDto>(report));

            //Act
            var app = new ReportApp(mock.Object, mockReportService.Object);
            var log = await app.LogReport(report);


            //Assert
            log.IsValid.Should().BeFalse();
            log.Errors.Should().Contain(x => $"O relatório precisa ser da semana atual, de {SystemConstants.BrazilTime.DateTime.GetStartOfWeek():dd/MM/yyyy} a {SystemConstants.BrazilTime.DateTime.GetEndOfWeek():dd/MM/yyyy}.".Equals(x.ErrorMessage));
        }
    }
}
