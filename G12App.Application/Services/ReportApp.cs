using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Globalization;
using System.Net;

namespace G12App.Application.Services
{
    public class ReportApp : IReportApp
    {
        private readonly ICellService _cellService;
        private readonly IReportService _reportService;

        public ReportApp(ICellService cellService, IReportService reportService)
        {
            _cellService = cellService;
            _reportService = reportService;
        }

        private static int GetWeekNum(DateTime dt)
        {
            var calendar = new CultureInfo("en-US").Calendar;
            var week = calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return week;
        }

        private static (DateTime FirstDay, DateTime LastDay) GetWeekSpan(DateTime dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
            var startDate = dt.AddDays(-1 * diff).Date;

            return (startDate, startDate.AddDays(6));
        }

        public async Task<ServiceResult<ReportDto>> LogReport(ReportDto reportDTO)
        {
            var (firstDay, lastDay) = GetWeekSpan(DateTime.Now);

            if (reportDTO.Date > DateTime.Now)
                return new ServiceResult<ReportDto>(null, HttpStatusCode.BadRequest, "O relatório não pode ser enviado para uma data futura.");

            if (GetWeekNum(reportDTO.Date) != GetWeekNum(DateTime.Now))
                return new ServiceResult<ReportDto>(null, HttpStatusCode.BadRequest, $"O relatório precisa ser da semana atual, de {firstDay:dd/MM/yyyy} a {lastDay:dd/MM/yyyy}.");

            var existentReport = await _reportService.GetAllReports(new Query<ReportFilter>()
            {
                Filter = new ReportFilter() { StartDate = firstDay, EndDate = lastDay, CellId = reportDTO.CellId },
                Pagination = Pagination.SingleRecord,
            });

            if ((existentReport.Data?.Any()).GetValueOrDefault())
                return new ServiceResult<ReportDto>(null, HttpStatusCode.BadRequest, $"O relatório para esta semana já foi enviado (#{existentReport.Data.FirstOrDefault().Id}).");

            var cellResult = await _cellService.GetCellById(reportDTO.CellId);
            if (cellResult == null)
                return new ServiceResult<ReportDto>(null, HttpStatusCode.BadRequest, "Célula não encontrada.");

            if (cellResult.Data.WeekDay > reportDTO.Date.DayOfWeek)
                return new ServiceResult<ReportDto>(null, HttpStatusCode.BadRequest, "O relatório não pode ser lançado antes da data de cadastro da célula.");

            if (cellResult.Data.WeekDay == reportDTO.Date.DayOfWeek && cellResult.Data.Time > TimeOnly.FromDateTime(reportDTO.Date))
                return new ServiceResult<ReportDto>(null, HttpStatusCode.BadRequest, $"O relatório não pode ser lançado antes do horário de cadastro da célula ({cellResult.Data.Time:HH:mm}).");

            return await _reportService.Add(reportDTO);
        }
    }
}
