using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Globalization;

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
            var startDayWeek = DateTime.Today.AddDays(
                (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek -
                (int)DateTime.Today.DayOfWeek);

            var endDayWeek = startDayWeek + TimeSpan.FromDays(7) - TimeSpan.FromSeconds(1);

            var totalMembers = await _memberRepository.CountMembersAsync(new Query<MemberFilter>() { });
            var totalCells = await _cellRepository.CountCellsAsync(new Query<CellFilter>() { });

            var weekReports = await _reportRepository.CountReportsAsync(
                new Query<ReportFilter>()
                {
                    Filter = new ReportFilter()
                    {
                        StartDate = startDayWeek,
                        EndDate = endDayWeek,
                    }
                });

            return new DashboardDto()
            {
                TotalMembers = totalMembers,
                TotalCells = totalCells,
                WeekReports = weekReports
            };
        }
    }
}
