using G12App.Domain.DTOs;
using G12App.Domain.Querying;
using G12App.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace G12App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IMemberService memberService, 
            [FromQuery] int page, [FromQuery] int limit, [FromQuery] string sortBy, [FromQuery] string sortOrder)
        {
            var query = new Query(page, limit, sortBy, sortOrder);
            var members = await memberService.GetMembers(query);
            return Ok(members);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromServices] IMemberService memberService, [FromBody] MemberDto member)
        {
            var result = await memberService.Add(member);
            return Ok(result);
        }

    }
}
