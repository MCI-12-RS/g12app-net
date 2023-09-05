using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G12App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IMemberService memberService)
        {
            var members = await memberService.GetMembers();
            return Ok(members);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromServices] IMemberService memberService, [FromBody] MemberDTO member)
        {
            var result = await memberService.Add(member);
            return Ok(member);
        }

    }
}
