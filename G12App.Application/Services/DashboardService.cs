using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Caching;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using G12App.Domain.Model.Enum;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.AspNetCore.Http;


namespace G12App.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ICellRepository _cellRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ICacheService _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardService(
            IMemberRepository memberRepository,
            ICellRepository cellRepository,
            IReportRepository reportRepository,
            IHttpContextAccessor httpContextAccessor,
            ICacheService cache)
        {
            _memberRepository = memberRepository;
            _cellRepository = cellRepository;
            _reportRepository = reportRepository;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<DashboardDto> GetDashbordData()
        {
            var startDayWeek = SystemConstants.BrazilTime.Date.GetStartOfWeek();
            var endDayWeek = SystemConstants.BrazilTime.Date.GetEndOfWeek();

            var reportFilter = new Query<ReportFilter>()
            {
                Filter = new ReportFilter()
                {
                    StartDate = startDayWeek,
                    EndDate = endDayWeek,
                }
            };

            return new DashboardDto()
            {
                TotalMembers =
                    await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                    {
                        Filter = new MemberFilter()
                        {
                            Idle = false
                        }
                    }),

                TotalDiscipleshipCells =
                    await _cellRepository.CountCellsAsync(new Query<CellFilter>()
                    {
                        Filter = new CellFilter
                        {
                            CellType = CellType.Discipleship
                        }
                    }),

                TotalEvangelisticCells =
                    await _cellRepository.CountCellsAsync(new Query<CellFilter>()
                    {
                        Filter = new CellFilter
                        {
                            CellType = CellType.Evangelistic
                        }
                    }),

                CellsPendingReports =
                    await _cellRepository.CountCellsAsync(new Query<CellFilter>()
                    {
                        Filter = new CellFilter()
                        {
                            ReportStatus = CellReportStatus.OverDueThisWeek
                        }
                    }),

                WeekReports =
                    await _reportRepository.CountReportsAsync(reportFilter)),

                WeekVisitors =
                    await _reportRepository.GetCellsVisitors(reportFilter)),

                WeekNewMembers =
                    await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                    {
                        Filter = new MemberFilter()
                        {
                            CreatedDateFrom = startDayWeek,
                            CreatedDateUntil = endDayWeek,
                        }
                    }),

                WeekNewCells =
                    await _cellRepository.CountCellsAsync(new Query<CellFilter>()
                    {
                        Filter = new CellFilter()
                        {
                            CreatedDateFrom = startDayWeek,
                            CreatedDateUntil = endDayWeek,
                        }
                    }),

                TotalNonBaptizedMembers =
                    await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                    {
                        Filter = new MemberFilter()
                        {
                            Baptized = false
                        }
                    }),

                TotalIdleMembers =
                    await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                    {
                        Filter = new MemberFilter()
                        {
                            Idle = true
                        }
                    }),

                TotalCellReportedSkipped =
                    await _reportRepository.CountReportsAsync(new Query<ReportFilter>()
                    {
                        Filter = new ReportFilter()
                        {
                            WeekCellSkipped = true,
                            StartDate = startDayWeek,
                            EndDate = endDayWeek,
                        }
                    }),

                TotalCellReportedCompleted =
                    await _reportRepository.CountReportsAsync(new Query<ReportFilter>()
                    {
                        Filter = new ReportFilter()
                        {
                            WeekCellSkipped = false,
                            StartDate = startDayWeek,
                            EndDate = endDayWeek,
                        }
                    }),

                TotalMembersWithoutLeadership =
                    await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                    {
                        Filter = new MemberFilter()
                        {
                            HasNoLeadership = true
                        }
                    }),

                TotalMembersWithNoCellLink =
                    await _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                    {
                        Filter = new MemberFilter()
                        {
                            CellLink = CellLink.None,
                        }
                    }),
            };
        }
    }
}
