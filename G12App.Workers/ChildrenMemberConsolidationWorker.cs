using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace G12App.Workers
{
    public class ChildrenMemberConsolidationWorker : BackgroundService
    {
        private readonly Query<MemberFilter> _defaultQuery;
        private readonly ILogger<ChildrenMemberConsolidationWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ChildrenMemberConsolidationWorker(
            IServiceProvider serviceProvider,
            ILogger<ChildrenMemberConsolidationWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            _defaultQuery = new Query<MemberFilter>()
            {
                Filter = new MemberFilter()
                {
                    ChildrenMembersComputed = false
                },
                Pagination = new Pagination(1, 5)
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                await using var scope = _serviceProvider.CreateAsyncScope();
                var memberService = scope.ServiceProvider.GetRequiredService<IMemberService>();
                try
                {
                    var members = await memberService.GetMembers(_defaultQuery);
                    foreach (var member in members.Data)
                    {
                        await ConsolidateChildrenMembersData(memberService, member);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "An error occurred while running worker.");
                }
            }
        }

        private async Task ConsolidateChildrenMembersData(IMemberService memberService, MemberDto member)
        {
            IEnumerable<MemberDto> childrenMembers;
            var page = 1;

            do
            {
                var result = await memberService.GetMembers(new Query<MemberFilter>()
                {
                    Filter = new MemberFilter()
                    {
                        LeadershipId = member.Id,
                        ChildrenMembersComputed = false
                    },
                    Pagination = new Pagination()
                    {
                        Limit = 10,
                        Page = page++,
                    }
                });

                childrenMembers = result.IsValid ? result.Data : new List<MemberDto>();

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