using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G12App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IReportService reportService,
            [FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string sortBy = "", [FromQuery] string sortOrder = "")
        {
            var query = new Query(page, limit, sortBy, sortOrder);
            var result = await reportService.GetAllReports(query);
            return result.AsActionResult();
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromServices] IReportService reportService, int id)
        {
            var result = await reportService.GetByReportId(id);
            return result.AsActionResult();
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromServices] IReportService reportService, [FromBody] ReportDTO reportDTO)
        {
            var result = await reportService.Add(reportDTO);
            return result.AsActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromServices] IReportService reportService, int id)
        {
            var result = await reportService.Delete(id); 
            return result.AsActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromServices] IReportService reportService, [FromBody] ReportDTO reportDTO, int id)
        {
            var result = await reportService.Update(reportDTO, id);
            return result.AsActionResult();
        }
    }
}
