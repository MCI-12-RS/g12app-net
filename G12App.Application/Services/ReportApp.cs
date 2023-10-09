using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;

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
            throw new NotImplementedException();
        }
    }
}
