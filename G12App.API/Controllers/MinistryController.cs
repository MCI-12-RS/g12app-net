using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MinistryController : ControllerBase
    {
        [HttpGet("{id}/couples")]
        public async Task<IActionResult> GetCouplesByParentAsync([FromServices] IMinistryService ministryService, int id, [FromQuery] int page, [FromQuery] int limit)
        {
            var result = await ministryService.GetCouplesByParentIdAsync(id, page, limit);
            return result.AsActionResult();
        }
    }
}
