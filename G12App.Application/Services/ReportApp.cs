using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
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

        public async Task <ServiceResult<ReportDTO>> LogReport(ReportDTO reportDTO)
        {
            //Validar data do ReportDTO < data atual
            if (reportDTO.Date < DateTime.Now)
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, "Insira uma data válida");

            //Buscar célula
            var cellResult = await _cellService.GetCellById(reportDTO.CellId);
            if (!cellResult.IsValid || cellResult.Data == null)
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, "Célula não encontrada");

            //Partindo do domingo validar dia da célula vs dia do report

            return new ServiceResult<ReportDTO>();





            //Mapear o DTO pra report 

        }
    }
}
