using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExportController : ControllerBase
    {
        [HttpPost("cell-reports")]
        public async Task<IActionResult> ExportReports(
            [FromServices] IExportService service, [FromBody] Query<ReportFilter> filter)
        {
            var cellFilter = new Query<CellFilter>() { };
            ServiceResult<FileDto> result = await service.ExportCellReports(cellFilter, filter);
            if (result.IsValid)
                return this.File(result.Data.Stream, result.Data.ContentType);

            return this.BadRequest();
        }
    }
}
