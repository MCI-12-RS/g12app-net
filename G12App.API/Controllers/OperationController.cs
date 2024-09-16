using G12App.Domain.DomainEvents;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Events;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OperationController : ControllerBase
    {
        [HttpPost("member-consolidation")]
        public async Task<ServiceResult> ConsolidateAllMembersData(
            [FromServices] IMemberService memberService,
            [FromServices] IEventDispatcher eventDispatcher)
        {
            var query = new Query<MemberFilter>()
            {
                Pagination = new Pagination(1, 20),
                Sort = new Sort(nameof(Member.MinistryLevel), false),
                Filter = new MemberFilter() { }
            };

            ServiceResult<IEnumerable<MemberDto>> members = null;
            do
            {
                members = await memberService.GetMembers(query);
                if (!members.IsValid || !(members.Data?.Any()).GetValueOrDefault())
                    break;

                foreach (var member in members.Data.Where(x => x.Id.HasValue))
                {
                    await eventDispatcher.DispatchEvent(new MemberDataConsolidationRequested(member));
                }

                query.Pagination.Page++;
            } while ((members?.Data?.Any()).GetValueOrDefault());

            return ServiceResult.Success;
        }

        [HttpGet("event-queue/count")]
        public int CountEvents([FromServices] IEventQueue eventQueue)
            => eventQueue.CountEvent();

        [HttpGet("event-queue/peek")]
        public string PeekNextEvent([FromServices] IEventQueue eventQueue)
            => eventQueue.PeekEvent()?.ToString();
    }
}
