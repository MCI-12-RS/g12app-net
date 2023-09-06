using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace G12App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] IMemberService memberService,
            [FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string sortBy = "", [FromQuery] string sortOrder = "")
        {
            var query = new Query(page, limit, sortBy, sortOrder);
            var members = await memberService.GetMembers(query);
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromServices] IMemberService memberService, int id)
        {
            var member = await memberService.GetMemberById(id);
            return Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromServices] IMemberService memberService, [FromBody] MemberDto member)
        {
            var result = await memberService.Add(member);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromServices] IMemberService memberService, [FromBody] MemberDto member, int id)
        {
            member.Id = id;
            var result = await memberService.Update(member, id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove([FromServices] IMemberService memberService, int id)
        {
            await memberService.Remove(id);
            return Ok();
        }


    }
}
