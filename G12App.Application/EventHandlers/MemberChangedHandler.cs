using G12App.Application.EventHandlers;
using G12App.Domain.DomainEvents;
using G12App.Domain.DTOs;
using G12App.Domain.Helpers;
using G12App.Domain.Interfaces.EventHandlers;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.Extensions.Logging;

namespace G12App.Application.Events
{
    public class MemberChangedHandler : BaseEventHandler<MemberChanged>, IMemberChangedHandler
    {
        private readonly IMemberService _memberService;
        private readonly IMemberRepository _memberRepository;
        private readonly ICellRepository _cellRepository;

        public MemberChangedHandler(
            IMemberService memberService,
            IMemberRepository memberRepository,
            ICellRepository cellRepository,
            ILogger<MemberChangedHandler> logger) : base(logger)
        {
            _memberService = memberService;
            _memberRepository = memberRepository;
            _cellRepository = cellRepository;
        }

        protected sealed override async Task HandleInternal(MemberChanged ev)
        {
            var isNew = ev.Before == null;
            var isDeleted = ev.After == null;

            var diff = new List<PropertyDifference>();
            if (!isNew && !isDeleted)
                diff = ObjectComparer.CompareObjects(ev.Before, ev.After);

            // Consolidate computed values for children
            var childrenConsolidationProps = new[] { nameof(MemberDto.Name), nameof(MemberDto.LeadershipId) };
            if (!isDeleted && (isNew || childrenConsolidationProps.Any(p => diff.Any(d => d.PropertyPath.Contains(p)))))
                await ConsolidateChildrenMembersData(ev.After);

            // Check if leadership was changed
            if (!isNew && !isDeleted && !diff.Any(d => d.PropertyPath.Contains(nameof(MemberDto.LeadershipId))))
                return;

            if (!isNew)
            {
                await ConsolidateMemberChildrenRecursively(ev.Before.LeadershipId.GetValueOrDefault());
                await ConsolidateMemberCellsRecursively(ev.Before.LeadershipId.GetValueOrDefault());
            }

            if (!isDeleted)
            {
                await ConsolidateMemberChildrenRecursively(ev.After.LeadershipId.GetValueOrDefault());
                await ConsolidateMemberCellsRecursively(ev.After.LeadershipId.GetValueOrDefault());
            }
        }

        private async Task ConsolidateMemberCellsRecursively(int leadershipId)
        {
            var currExec = 0;
            while (leadershipId > 0 && ++currExec <= 5)
            {
                var leader = await _memberRepository.GetMemberByIdAsync(leadershipId);

                var cellCount = await _cellRepository.CountCellsAsync(new Query<CellFilter>()
                {
                    Filter = new CellFilter()
                    {
                        LeadIdAllLevels = leader.Id
                    }
                });

                leader.TotalCells = cellCount;

                await _memberRepository.Update(leader);

                leadershipId = leader.LeadershipId ?? 0;
            }
        }

        private async Task ConsolidateMemberChildrenRecursively(int leadershipId)
        {
            var currExec = 0;
            while (leadershipId > 0 && ++currExec <= 5)
            {
                var leader = await _memberRepository.GetMemberByIdAsync(leadershipId);

                var childrenCount = await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                {
                    Filter = new MemberFilter()
                    {
                        LeadershipIdAllLevels = leader.Id
                    }
                });

                leader.TotalDisciples = childrenCount;

                await _memberRepository.Update(leader);

                leadershipId = leader.LeadershipId ?? 0;
            }
        }

        private async Task ConsolidateChildrenMembersData(MemberDto member)
        {
            IEnumerable<MemberDto> childrenMembers;
            var page = 1;

            await _memberService.ConsolidateMemberComputedData(member.Id.GetValueOrDefault());

            do
            {
                var result = await _memberService.GetMembers(new Query<MemberFilter>()
                {
                    Filter = new MemberFilter()
                    {
                        LeadershipId = member.Id
                    },
                    Pagination = new Pagination()
                    {
                        Limit = 10,
                        Page = page++,
                    }
                });

                childrenMembers = result.IsValid ? result.Data : [];

                foreach (var children in result.Data.Where(x => x.Id.HasValue))
                {
                    await _memberService.ConsolidateMemberComputedData(children.Id.GetValueOrDefault());
                    await ConsolidateChildrenMembersData(children);
                }

            } while (childrenMembers.Any());

            await _memberService.SetConsolidatedChildrenData(member.Id, true);
        }
    }
}
