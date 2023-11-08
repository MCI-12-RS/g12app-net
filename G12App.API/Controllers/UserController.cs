using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromServices] IUserService userService, int id)
        {
            var result = await userService.GetUserById(id);
            return result.AsActionResult();
        }
    }
}
