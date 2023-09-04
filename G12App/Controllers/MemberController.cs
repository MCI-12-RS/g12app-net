using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using G12App.Domain.RequestMessages;
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
        public IActionResult Post(MemberRequest member)
        {
            return Ok(member);
        }

    }
}
