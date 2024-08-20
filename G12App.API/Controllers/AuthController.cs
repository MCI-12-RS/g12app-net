using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromServices] IAuthService authService, [FromBody] LoginDto loginDTO)
        {
            var result = await authService.Login(loginDTO);
            return result.AsActionResult();
        }
    }
}
