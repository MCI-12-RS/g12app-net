using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IExportService
    {
        Task<ServiceResult<FileDto>> ExportCellReports(Query<CellFilter> cellQuery, Query<ReportFilter> reportQuery);
    }
}
