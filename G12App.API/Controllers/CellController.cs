using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CellController : ControllerBase
    {
        [HttpPost("search")]
        public async Task<IActionResult> Get([FromServices] ICellService cellService, [FromBody] Query<CellFilter> query)
        {
            query = query ?? throw new ArgumentNullException(nameof(query));
            var result = await cellService.GetCell(query);
            return result.AsActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromServices] ICellService cellService, int? id)
        {
            var result = await cellService.GetCellById(id);
            return result.AsActionResult();
        }

        [HttpGet("{id}/reports")]
        public async Task<IActionResult> GetReportsById([FromServices] IReportService reportService, int? id)
        {
            var result = await reportService.GetAllReports(new Query<ReportFilter>()
            {
                Pagination = new Pagination(1, 3),
                Filter = new ReportFilter()
                {
                    CellId = id,
                }
            });
            return result.AsActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromServices] ICellService cellService, [FromBody] CellDTO cell)
        {
            var result = await cellService.Add(cell);
            return result.AsActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromServices] ICellService cellService, int? id)
        {
            var result = await cellService.Remove(id);
            return result.AsActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromServices] ICellService cellService,[FromBody] CellDTO cell, int id)
        {
            cell.Id = id;
            var result = await cellService.Update(cell, id);
            return result.AsActionResult();
        }
    }
}
