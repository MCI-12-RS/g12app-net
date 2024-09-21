using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Caching;
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
        private readonly ICacheService _cache;

        public DashboardService(
            IMemberRepository memberRepository,
            ICellRepository cellRepository,
            IReportRepository reportRepository,
            ICacheService cache)
        {
            _memberRepository = memberRepository;
            _cellRepository = cellRepository;
            _reportRepository = reportRepository;
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
                    await _cache.GetOrFetch(CacheKeys.Member_CountActiveMembers,
                        () => _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                        {
                            Filter = new MemberFilter()
                            {
                                Idle = false
                            }
                        })),

                TotalDiscipleshipCells =
                    await _cache.GetOrFetch(CacheKeys.Cell_CountTotalDiscipleshipCells,
                        () => _cellRepository.CountCellsAsync(new Query<CellFilter>()
                        {
                            Filter = new CellFilter
                            {
                                CellType = CellType.Discipleship
                            }
                        })),

                TotalEvangelisticCells =
                    await _cache.GetOrFetch(CacheKeys.Cell_CountTotalEvangelisticCells,
                        () => _cellRepository.CountCellsAsync(new Query<CellFilter>()
                        {
                            Filter = new CellFilter
                            {
                                CellType = CellType.Evangelistic
                            }
                        })),

                CellsPendingReports =
                    await _cache.GetOrFetch(CacheKeys.Cell_CountPendingReports,
                        () => _cellRepository.CountCellsAsync(new Query<CellFilter>()
                        {
                            Filter = new CellFilter()
                            {
                                ReportStatus = CellReportStatus.OverDueThisWeek
                            }
                        })),

                WeekReports =
                    await _cache.GetOrFetch(CacheKeys.Cell_CountWeekReports,
                        () => _reportRepository.CountReportsAsync(reportFilter)),

                WeekVisitors =
                    await _cache.GetOrFetch(CacheKeys.Cell_CountWeekVisitors,
                        () => _reportRepository.GetCellsVisitors(reportFilter)),

                WeekNewMembers =
                    await _cache.GetOrFetch(CacheKeys.Member_CountCreatedCurrentWeek,
                        () => _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                        {
                            Filter = new MemberFilter()
                            {
                                CreatedDateFrom = startDayWeek,
                                CreatedDateUntil = endDayWeek,
                            }
                        })),

                WeekNewCells =
                    await _cache.GetOrFetch(CacheKeys.Cell_CountWeekNewCells,
                    () => _cellRepository.CountCellsAsync(new Query<CellFilter>()
                    {
                        Filter = new CellFilter()
                        {
                            CreatedDateFrom = startDayWeek,
                            CreatedDateUntil = endDayWeek,
                        }
                    })),

                TotalNonBaptizedMembers =
                    await _cache.GetOrFetch(CacheKeys.Member_CountNonBaptized,
                        () => _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                        {
                            Filter = new MemberFilter()
                            {
                                Baptized = false
                            }
                        })),

                TotalIdleMembers =
                    await _cache.GetOrFetch(CacheKeys.Member_CountIdle,
                        () => _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                        {
                            Filter = new MemberFilter()
                            {
                                Idle = true
                            }
                        })),

                TotalCellReportedSkipped =
                    await _cache.GetOrFetch(CacheKeys.Cell_CountReportSkipped,
                        () => _reportRepository.CountReportsAsync(new Query<ReportFilter>()
                        {
                            Filter = new ReportFilter()
                            {
                                WeekCellSkipped = true,
                                StartDate = startDayWeek,
                                EndDate = endDayWeek,
                            }
                        })),

                TotalCellReportedCompleted =
                    await _cache.GetOrFetch(CacheKeys.Cell_CountReportedCompleted,
                        () => _reportRepository.CountReportsAsync(new Query<ReportFilter>()
                        {
                            Filter = new ReportFilter()
                            {
                                WeekCellSkipped = false,
                                StartDate = startDayWeek,
                                EndDate = endDayWeek,
                            }
                        })),

                TotalMembersWithoutLeadership =
                    await _cache.GetOrFetch(CacheKeys.Member_CountWithoutLeadership,
                    () => _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                    {
                        Filter = new MemberFilter()
                        {
                            HasNoLeadership = true
                        }
                    })),

                TotalMembersWithNoCellLink =
                    await _cache.GetOrFetch(CacheKeys.Member_CountWithNoCellLink,
                        () => _memberRepository.CountMembersAsync(new Query<MemberFilter>()
                        {
                            Filter = new MemberFilter()
                            {
                                CellLink = CellLink.None,
                            }
                        })),
            };
        }
    }
}
