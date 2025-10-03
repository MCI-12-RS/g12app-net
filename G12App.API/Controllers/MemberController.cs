using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberController : ControllerBase
    {
        [HttpPost("search")]
        public async Task<IActionResult> SearchAsync([FromServices] IMemberService memberService, [FromBody] Query<MemberFilter> query)
        {
            query = query ?? throw new ArgumentNullException(nameof(query));
            var result = await memberService.GetMembers(query);
            return result.AsActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] IMemberService memberService, int id)
        {
            var result = await memberService.GetMemberById(id);
            return result.AsActionResult();
        }

        [HttpGet("{id}/children")]
        public async Task<IActionResult> GetByParentIdAsync([FromServices] IMemberService memberService, int parentId, [FromQuery] int page, [FromQuery] int limit)
        {
            var result = await memberService.GetMembersByParentId(parentId, page, limit);
            return result.AsActionResult();
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.AppAdmin},{Roles.AppSuperAdmin}")]
        public async Task<IActionResult> PostAsync([FromServices] IMemberService memberService, [FromBody] MemberDto member)
        {
            var result = await memberService.Add(member);
            return result.AsActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromServices] IMemberService memberService, [FromBody] MemberDto member, int id)
        {
            member.Id = id;
            var result = await memberService.Update(member, id);
            return result.AsActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{Roles.AppAdmin},{Roles.AppSuperAdmin}")]
        public async Task<IActionResult> RemoveAsync([FromServices] IMemberService memberService, int id)
        {
            var result = await memberService.Remove(id);
            return result.AsActionResult();
        }
    }
}
