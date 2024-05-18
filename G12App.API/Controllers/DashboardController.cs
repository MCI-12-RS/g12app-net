using G12App.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IDashboardService dashboardService)
        {
            return Ok(await dashboardService.GetDashbordData());
        }
    }
}
