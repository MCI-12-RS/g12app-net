using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
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

        private static (DateTime, DateTime) GetWeekSpan(DateTime dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
            var startDate = dt.AddDays(-1 * diff).Date;

            return (startDate, startDate.AddDays(6));
        }

        public async Task<ServiceResult<ReportDTO>> LogReport(ReportDTO reportDTO)
        {
            if (reportDTO.Date > DateTime.Now)
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, "O relatório não pode ser enviado para uma data futura.");

            var cellResult = await _cellService.GetCellById(reportDTO.CellId);
            if (cellResult == null)
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, "Célula não encontrada.");

            if (cellResult.Data.WeekDay > reportDTO.Date.DayOfWeek)
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, "O relatório não pode ser lançado antes da data de cadastro da célula.");

            if (cellResult.Data.WeekDay == reportDTO.Date.DayOfWeek && cellResult.Data.Time > TimeOnly.FromDateTime(reportDTO.Date))
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, $"O relatório não pode ser lançado antes do horário de cadastro da célula ({cellResult.Data.Time.ToString("HH:mm")}).");

            if (GetWeekNum(reportDTO.Date) != GetWeekNum(DateTime.Now))
            {
                var dates = GetWeekSpan(DateTime.Now);
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, $"O relatório precisa ser da semana atual, de {dates.Item1.ToString("dd/MM/yyyy")} a {dates.Item2.ToString("dd/MM/yyyy")}.");
            }

            return await _reportService.Add(reportDTO);
        }
    }
}
