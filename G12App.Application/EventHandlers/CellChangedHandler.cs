using G12App.Application.EventHandlers;
using G12App.Domain.DomainEvents;
using G12App.Domain.DTOs;
using G12App.Domain.Helpers;
using G12App.Domain.Interfaces.EventHandlers;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.Extensions.Logging;

namespace G12App.Application.Events
{
    public class CellChangedHandler : BaseEventHandler<CellChanged>, ICellChangedHandler
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ICellRepository _cellRepository;

        public CellChangedHandler(
            IMemberRepository memberRepository,
            ICellRepository cellRepository,
            ILogger<CellChangedHandler> logger) : base(logger)
        {
            _memberRepository = memberRepository;
            _cellRepository = cellRepository;
        }

        protected sealed override async Task HandleInternal(CellChanged ev)
        {
            var isNew = ev.Before == null;
            var isDeleted = ev.After == null;

            var diff = ObjectComparer.CompareObjects(ev.Before, ev.After);

            // Check if leadership was changed
            if (!diff.Any(d => d.PropertyPath.Contains(nameof(CellDto.LeadId))))
                return;

            if (ev.Before != null)
                await ConsolidateMemberCellsRecurrently(ev.Before.LeadId);

            await ConsolidateMemberCellsRecurrently(ev.After.LeadId);
        }

        private async Task ConsolidateMemberCellsRecurrently(int leadershipId)
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
    }
}
