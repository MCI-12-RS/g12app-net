using G12App.Domain.DTOs;
using G12App.Domain.Helpers;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace G12App.Application.Events
{
    public class CellUpserted : BaseEvent
    {
        private readonly CellDto _before;
        private readonly CellDto _after;

        protected override object EntityId => _after?.Id ?? _before?.Id;

        public CellUpserted(CellDto before, CellDto after)
            => (_before, _after) = (before, after);

        protected override async Task HandleInternal(IServiceProvider serviceProvider, CancellationToken token)
        {
            var memberRepository = serviceProvider.GetRequiredService<IMemberRepository>();
            var cellRepository = serviceProvider.GetRequiredService<ICellRepository>();
            var isNew = _before == null;
            var isDeleted = _after == null;

            var diff = ObjectComparer.CompareObjects(_before, _after);

            // Check if leadership was changed
            if (!diff.Any(d => d.PropertyPath.Contains(nameof(CellDto.LeadId))))
                return;

            if (_before != null)
                await ConsolidateMemberCellsRecurrently(cellRepository, memberRepository, _before.LeadId);

            await ConsolidateMemberCellsRecurrently(cellRepository, memberRepository, _after.LeadId);
        }

        private async Task ConsolidateMemberCellsRecurrently(
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
    }
}
