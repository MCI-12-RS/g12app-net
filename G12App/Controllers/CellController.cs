using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G12App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CellController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] ICellService cellService,
            [FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string sortBy = "", [FromQuery] string sortOrder = "")
        {
            var query = new Query(page, limit, sortBy, sortOrder);
            var result = await cellService.GetCell(query);
            return Ok(result);
        }
    }
}
