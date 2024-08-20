using G12App.Domain.DTOs;
using G12App.Domain.Helpers;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace G12App.Application.Events
{
    public class MemberUpserted : BaseEvent
    {
        private readonly MemberDto _before;
        private readonly MemberDto _after;

        protected override object EntityId => _after?.Id ?? _before?.Id;

        public MemberUpserted(MemberDto before, MemberDto after)
            => (_before, _after) = (before, after);

        protected override async Task HandleInternal(IServiceProvider serviceProvider, CancellationToken token)
        {
            var memberService = serviceProvider.GetRequiredService<IMemberService>();
            var memberRepository = serviceProvider.GetRequiredService<IMemberRepository>();
            var cellRepository = serviceProvider.GetRequiredService<ICellRepository>();
            var isNew = _before == null;
            var isDeleted = _after == null;

            var diff = new List<PropertyDifference>();
            if (!isNew && !isDeleted)
                diff = ObjectComparer.CompareObjects(_before, _after);

            // Consolidate computed values for children
            var childrenConsolidationProps = new[] { nameof(MemberDto.Name), nameof(MemberDto.LeadershipId) };
            if (!isDeleted && (isNew || childrenConsolidationProps.Any(p => diff.Any(d => d.PropertyPath.Contains(p)))))
                await ConsolidateChildrenMembersData(memberService, _after);

            // Check if leadership was changed
            if (!isNew && !isDeleted && !diff.Any(d => d.PropertyPath.Contains(nameof(MemberDto.LeadershipId))))
                return;

            if (!isNew)
            {
                await ConsolidateMemberChildrenRecursively(memberRepository, _before.LeadershipId.GetValueOrDefault());
                await ConsolidateMemberCellsRecursively(cellRepository, memberRepository, _before.LeadershipId.GetValueOrDefault());
            }

            if (!isDeleted)
            {
                await ConsolidateMemberChildrenRecursively(memberRepository, _after.LeadershipId.GetValueOrDefault());
                await ConsolidateMemberCellsRecursively(cellRepository, memberRepository, _after.LeadershipId.GetValueOrDefault());
            }
        }

        private async Task ConsolidateMemberCellsRecursively(
            ICellRepository cellRepository, IMemberRepository memberRepository, int leadershipId)
        {
            var currExec = 0;
            while (leadershipId > 0 && ++currExec <= 5)
            {
                var leader = await memberRepository.GetMemberByIdAsync(leadershipId);

                var cellCount = await cellRepository.CountCellsAsync(new Query<CellFilter>()
                {
                    Filter = new CellFilter()
                    {
                        LeadIdAllLevels = leader.Id
                    }
                });

                leader.TotalCells = cellCount;

                await memberRepository.Update(leader);

                leadershipId = leader.LeadershipId ?? 0;
            }
        }

        private async Task ConsolidateMemberChildrenRecursively(IMemberRepository memberRepository, int leadershipId)
        {
            var currExec = 0;
            while (leadershipId > 0 && ++currExec <= 5)
            {
                var leader = await memberRepository.GetMemberByIdAsync(leadershipId);

                var childrenCount = await memberRepository.CountMembersAsync(new Query<MemberFilter>()
                {
                    Filter = new MemberFilter()
                    {
                        LeadershipIdAllLevels = leader.Id
                    }
                });

                leader.TotalDisciples = childrenCount;

                await memberRepository.Update(leader);

                leadershipId = leader.LeadershipId ?? 0;
            }
        }

        private async Task ConsolidateChildrenMembersData(
            IMemberService memberService, MemberDto member)
        {
            IEnumerable<MemberDto> childrenMembers;
            var page = 1;

            await memberService.ConsolidateMemberComputedData(member.Id.GetValueOrDefault());

            do
            {
                var result = await memberService.GetMembers(new Query<MemberFilter>()
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
                    await memberService.ConsolidateMemberComputedData(children.Id.GetValueOrDefault());
                    await ConsolidateChildrenMembersData(memberService, children);
                }

            } while (childrenMembers.Any());

            await memberService.SetConsolidatedChildrenData(member.Id, true);
        }
    }
}
