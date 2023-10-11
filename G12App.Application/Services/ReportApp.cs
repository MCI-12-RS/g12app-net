using AutoMapper;
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
        private readonly IMapper _mapper;

        public ReportApp(ICellService cellService, IReportService reportService)
        {
            _cellService = cellService;
            _reportService = reportService;
        }

        public async Task<ServiceResult<ReportDTO>> LogReport(ReportDTO reportDTO)
        {
            //Validar data do ReportDTO < data atual
            if (reportDTO.Date > DateTime.Now)
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, "O relatório não pode ser enviado antes da célula");

            //Buscar célula
            var cellResult = await _cellService.GetCellById(reportDTO.CellId);
            if (cellResult == null)
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, "Célula não encontrada");

            //Partindo do domingo validar dia da célula vs dia do report
            

            if (cellResult.Data.WeekDay > reportDTO.Date.DayOfWeek)
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, "Relatório não pode ser lançado antes da data de cadastro da célula");

            if (cellResult.Data.WeekDay == reportDTO.Date.DayOfWeek && cellResult.Data.Time > TimeOnly.FromDateTime(reportDTO.Date))
                return new ServiceResult<ReportDTO>(null, HttpStatusCode.BadRequest, "Relatório não pode ser lançado antes do horário de cadastro da célula");

            //Mapear o DTO pra report 
            return await _reportService.Add(reportDTO);
        }
    }
}
