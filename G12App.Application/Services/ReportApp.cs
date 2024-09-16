using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
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

        public async Task<ServiceResult<ReportDto>> LogReport(ReportDto reportDTO)
        {
            var weekStart = SystemConstants.BrazilTime.DateTime.GetStartOfWeek();
            var weekEnd = SystemConstants.BrazilTime.DateTime.GetEndOfWeek();

            if (reportDTO.Date > SystemConstants.BrazilTime.DateTime)
                return new ServiceResult<ReportDto>(null, HttpStatusCode.BadRequest, "O relatório não pode ser enviado para uma data futura.");

            if (reportDTO.Date < weekStart)
                return new ServiceResult<ReportDto>(null, HttpStatusCode.BadRequest, $"O relatório precisa ser da semana atual, de {weekStart:dd/MM/yyyy} a {weekEnd:dd/MM/yyyy}.");

            var existentReport = await _reportService.GetAllReports(new Query<ReportFilter>()
            {
                Filter = new ReportFilter() { StartDate = weekStart, EndDate = weekEnd, CellId = reportDTO.CellId },
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
