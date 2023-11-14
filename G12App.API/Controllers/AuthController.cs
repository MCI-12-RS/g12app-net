using G12App.Domain.Model;
using G12App.API;
using G12App.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.DTOs;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
       
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromServices] IAuthService authService, [FromBody] LoginDTO loginDTO)
        {
            var result = await authService.Login(loginDTO);
            return result.AsActionResult();
        }
    }
}
