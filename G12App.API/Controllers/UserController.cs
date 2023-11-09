using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("search")]
        public async Task<IActionResult> Get([FromServices] IUserService userService, [FromBody] Query<UserFilter> query)
        {
            query = query ?? throw new ArgumentNullException(nameof(query));
            var result = await userService.GetUsers(query);
            return result.AsActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromServices] IUserService userService, [FromBody]  UserDTO userDTO)
        {
            var result = await userService.Add(userDTO);
            return result.AsActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromServices] IUserService userService, int id)
        {
            var result = await userService.GetUserById(id);
            return result.AsActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromServices] IUserService userService, int id)
        {
            var result = await userService.Remove(id);
            return result.AsActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromServices] IUserService userService, [FromBody] UserDTO userDTO, int id)
        {
            var result = await userService.Update(userDTO, id);
            return result.AsActionResult();
        }
    }
}
