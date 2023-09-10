using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using G12App.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using G12App.Domain.DTOs;

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
            return result.AsActionResult();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromServices] ICellService cellService, int? id)
        {
            var result = await cellService.GetCellById(id);
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
        public async Task<IActionResult> Update([FromServices] ICellService cellService, CellDTO cell, int id)
        {
            cell.Id = id;
            var result = await cellService.Update(cell, id);
            return result.AsActionResult();
        }
    }
}
