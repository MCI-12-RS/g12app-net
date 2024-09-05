using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.EntityFrameworkCore;

namespace G12App.Application.Services
{
    public class ExportService : IExportService
    {
        private readonly ICellRepository _cellRepository;
        private readonly IExcelService _excelService;
        private readonly IColumnFormatter _formatter;

        public ExportService(
            ICellRepository cellRepository,
            IColumnFormatter formatter,
            IExcelService excelService)
        {
            _cellRepository = cellRepository;
            _excelService = excelService;
            _formatter = formatter;
        }

        public async Task<ServiceResult<FileDto>> ExportCellReports(Query<CellFilter> cellQuery, Query<ReportFilter> reportQuery)
        {
            var query = _cellRepository
                            .GetCellsReports(cellQuery, reportQuery);

            var cellList = await query.ToListAsync();

            var formatter = _formatter.GetCellReportFormatter();

            var fileSource = new FileSourceDto()
            {
                Columns = formatter.Keys.ToList(),
                Data = cellList
                        .OrderBy(x => (x.Cell.Lead.ComputedG12LeaderId == 0 ? null : x.Cell.Lead.ComputedG12LeaderId) ?? x.Cell.Lead.Id)
                        .Select(x => formatter.Select(y => y.Value.Invoke(x)).ToList())
                        .ToList()
            };

            var fileResult = _excelService.CreateFile(fileSource);

            return fileResult;
        }
    }
}
