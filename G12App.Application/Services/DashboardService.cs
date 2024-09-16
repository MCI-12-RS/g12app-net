using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using G12App.Domain.Model.Enum;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;


namespace G12App.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ICellRepository _cellRepository;
        private readonly IReportRepository _reportRepository;

        public DashboardService(
            IMemberRepository memberRepository,
            ICellRepository cellRepository,
            IReportRepository reportRepository)
        {
            _memberRepository = memberRepository;
            _cellRepository = cellRepository;
            _reportRepository = reportRepository;
        }

        public async Task<DashboardDto> GetDashbordData()
        {
            var startDayWeek = SystemConstants.BrazilTime.Date.GetStartOfWeek();
            var endDayWeek = SystemConstants.BrazilTime.Date.GetEndOfWeek();

            var memberFilter = new Query<MemberFilter>() { };
            var cellFilter = new Query<CellFilter>() { };
            var reportFilter = new Query<ReportFilter>()
            {
                Filter = new ReportFilter()
                {
                    StartDate = startDayWeek,
                    EndDate = endDayWeek,
                }
            };

            var totalMembers = await _memberRepository.CountMembersAsync(memberFilter);
            var totalCells = await _cellRepository.CountCellsAsync(cellFilter);
            var cellsPendingReports = _cellRepository.GetCellsReports(cellFilter, reportFilter, false).Count();
            var weekReports = await _reportRepository.CountReportsAsync(reportFilter);
            var weekVisitors = await _reportRepository.GetCellsVisitors(reportFilter);

            var weekNewMembers = await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
            {
                Filter = new MemberFilter()
                {
                    CreatedDateFrom = startDayWeek,
                    CreatedDateUntil = endDayWeek,
                }
            });

            var weekNewCells = await _cellRepository.CountCellsAsync(new Query<CellFilter>()
            {
                Filter = new CellFilter()
                {
                    CreatedDateFrom = startDayWeek,
                    CreatedDateUntil = endDayWeek,
                }
            });

            var totalNonBaptizedMembers = await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
            {
                Filter = new MemberFilter()
                {
                    Baptized = false
                }
            });

            var totalIdleMembers = await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
            {
                Filter = new MemberFilter()
                {
                    Idle = true
                }
            });

            var totalMembersWithoutLeadership = await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
            {
                Filter = new MemberFilter()
                {
                    HasNoLeadership = true
                }
            });

            var totalCellReportedSkipped = await _reportRepository.CountReportsAsync(new Query<ReportFilter>()
            {
                Filter = new ReportFilter()
                {
                    WeekCellSkipped = true,
                    StartDate = startDayWeek,
                    EndDate = endDayWeek,
                }
            });

            var totalCellReportedCompleted = await _reportRepository.CountReportsAsync(new Query<ReportFilter>()
            {
                Filter = new ReportFilter()
                {
                    WeekCellSkipped = false,
                    StartDate = startDayWeek,
                    EndDate = endDayWeek,
                }
            });

            var totalMembersWithNoCellLink = await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
            {
                Filter = new MemberFilter()
                {
                    CellLink = CellLink.None,
                }
            });

            return new DashboardDto()
            {
                TotalMembers = totalMembers,
                TotalCells = totalCells,
                CellsPendingReports = cellsPendingReports,
                WeekReports = weekReports,
                WeekVisitors = weekVisitors,
                WeekNewMembers = weekNewMembers,
                WeekNewCells = weekNewCells,
                TotalNonBaptizedMembers = totalNonBaptizedMembers,
                TotalIdleMembers = totalIdleMembers,
                TotalCellReportedSkipped = totalCellReportedSkipped,
                TotalCellReportedCompleted = totalCellReportedCompleted,
                TotalMembersWithoutLeadership = totalMembersWithoutLeadership,
                TotalMembersWithNoCellLink = totalMembersWithNoCellLink,
            };
        }
    }
}
