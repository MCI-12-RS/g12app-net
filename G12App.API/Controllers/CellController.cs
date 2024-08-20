using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using G12App.Domain.Model;

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
        public async Task<IActionResult> GetLastReportsByCellId([FromServices] IReportService reportService, int? id)
        {
            var result = await reportService.GetAllReports(new Query<ReportFilter>()
            {
                Pagination = new Pagination(1, 3),
                Sort = new Sort("CreationDate", true),
                Filter = new ReportFilter()
                {   
                    CellId = id,
                }
            });
            return result.AsActionResult();
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.AppAdmin},{Roles.AppSuperAdmin}")]
        public async Task<IActionResult> Add([FromServices] ICellService cellService, [FromBody] CellDto cell)
        {
            var result = await cellService.Add(cell);
            return result.AsActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{Roles.AppAdmin},{Roles.AppSuperAdmin}")]
        public async Task<IActionResult> Remove([FromServices] ICellService cellService, int? id)
        {
            var result = await cellService.Remove(id);
            return result.AsActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{Roles.AppAdmin},{Roles.AppSuperAdmin}")]
        public async Task<IActionResult> Update([FromServices] ICellService cellService,[FromBody] CellDto cell, int id)
        {
            cell.Id = id;
            var result = await cellService.Update(cell, id);
            return result.AsActionResult();
        }
    }
}
