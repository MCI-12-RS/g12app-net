using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        [HttpPost("search")]
        public async Task<IActionResult> Get([FromServices] IReportService reportService, [FromBody] Query<ReportFilter> query)
        {
            query = query ?? throw new ArgumentNullException(nameof(query));
            var result = await reportService.GetReportsList(query);
            return result.AsActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromServices] IReportService reportService, int id)
        {
            var result = await reportService.GetById(id);
            return result.AsActionResult();
        }

        [HttpPost("log")]
        public async Task<IActionResult> PostReport([FromServices] IReportApp reportApp, [FromBody] ReportDto reportDTO)
        {
            var result = await reportApp.LogReport(reportDTO);
            return result.AsActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{Roles.AppAdmin},{Roles.AppSuperAdmin}")]
        public async Task<IActionResult> Delete([FromServices] IReportService reportService, int id)
        {
            var result = await reportService.Delete(id);
            return result.AsActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{Roles.AppAdmin},{Roles.AppSuperAdmin}")]
        public async Task<IActionResult> Update([FromServices] IReportService reportService, [FromBody] ReportDto reportDTO, int id)
        {
            var result = await reportService.Update(reportDTO, id);
            return result.AsActionResult();
        }
    }
}
