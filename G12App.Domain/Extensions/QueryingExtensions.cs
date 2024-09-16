using G12App.Domain.Interfaces.Strategies;
using G12App.Domain.Model;
using G12App.Domain.Model.Base;
using G12App.Domain.Model.Enum;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace G12App.Domain.Extensions
{
    public static class QueryingExtensions
    {
        public static IQueryable<TEntity> ApplyStrategy<TEntity>(this IQueryable<TEntity> queryable, IBaseFetchStrategy<TEntity> strategy)
            where TEntity : BaseEntity
        {
            return strategy.Setup(queryable);
        }

        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> queryable, Query query)
        {
            var pagination = query.Pagination;

            if (pagination.Page == 0 && pagination.Limit == 0)
                pagination = Pagination.SingleRecord;

            return queryable.Skip((pagination.Page - 1) * pagination.Limit).Take(pagination.Limit);
        }

        public static IOrderedQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> queryable, Query query)
            where TEntity : BaseEntity
        {
            if (string.IsNullOrWhiteSpace(query.Sort.SortBy))
                return queryable.OrderBy(x => x.Id);

            var command = query.Sort.Order ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperties()
                .FirstOrDefault(x => x.Name.Equals(query.Sort.SortBy, StringComparison.InvariantCultureIgnoreCase));
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, [type, property.PropertyType],
                queryable.Expression, Expression.Quote(orderByExpression));

            return queryable.Provider.CreateQuery<TEntity>(resultExpression) as IOrderedQueryable<TEntity>;
        }

        #region Member

        public static IQueryable<Member> ApplyFilter(this IQueryable<Member> queryable, Query<MemberFilter> query)
        {
            if (query == null)
                return queryable;

            if (!string.IsNullOrWhiteSpace(query.Filter.Name))
                queryable = queryable.Where(x => x.Name.ToLower().Contains(query.Filter.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.UserName))
                queryable = queryable.Where(x => x.MobileNumber.Equals(query.Filter.UserName) || x.Email.ToLower().Equals(query.Filter.UserName.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.LeadershipName))
                queryable = queryable.Where(x => x.LeadershipTree.ToLower().Contains(query.Filter.LeadershipName.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.Email))
                queryable = queryable.Where(x => x.Email.ToLower().Contains(query.Filter.Email.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_Neighborhood))
                queryable = queryable.Where(x => x.HomeAddress.Neighborhood.ToLower().Contains(query.Filter.HomeAddress_Neighborhood.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_City))
                queryable = queryable.Where(x => x.HomeAddress.City.ToLower().Contains(query.Filter.HomeAddress_City.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_State))
                queryable = queryable.Where(x => x.HomeAddress.State.ToLower().Contains(query.Filter.HomeAddress_State.ToLower()));

            if (query.Filter.MinistryLevel.HasValue && query.Filter.MinistryLevel.Value >= 0)
                queryable = queryable.Where(x => x.MinistryLevel.Equals(query.Filter.MinistryLevel.Value));

            if ((query.Filter.MinistryLevels?.Any()).GetValueOrDefault())
                queryable = queryable.Where(x => query.Filter.MinistryLevels.Contains(x.MinistryLevel.Value));

            if (query.Filter.NotInId != null && query.Filter.NotInId.Any())
                queryable = queryable.Where(x => !query.Filter.NotInId.Contains(x.Id));

            if (query.Filter.ChildrenMembersComputed.HasValue)
                queryable = queryable.Where(x => query.Filter.ChildrenMembersComputed.Equals(x.ChildrenMembersComputed));

            if (query.Filter.LeadershipId.HasValue)
                queryable = queryable.Where(x => query.Filter.LeadershipId.Equals(x.LeadershipId));

            if (query.Filter.LeadershipIdAllLevels.HasValue)
                queryable = queryable.Where(x => x.LeadershipIdTree.Contains($"|{query.Filter.LeadershipIdAllLevels}|"));

            if (query.Filter.CreatedDateFrom.HasValue)
                queryable = queryable.Where(x => x.CreationDate >= query.Filter.CreatedDateFrom.Value);

            if (query.Filter.CreatedDateUntil.HasValue)
                queryable = queryable.Where(x => x.CreationDate <= query.Filter.CreatedDateUntil.Value);

            if (query.Filter.Idle.HasValue)
                queryable = queryable.Where(x => query.Filter.Idle.Equals(x.Idle));

            if (query.Filter.Baptized.HasValue)
                queryable = queryable.Where(x => query.Filter.Baptized.Equals(x.Baptized));

            if (query.Filter.HasNoLeadership.HasValue)
                queryable = queryable.Where(x => query.Filter.HasNoLeadership.Equals(x.HasNoLeadership));

            if (query.Filter.CellLink.HasValue)
            {
                var sincePeriod = SystemConstants.BrazilTime
                                    .Subtract(TimeSpan.FromDays(30))
                                    .DateTime;

                switch (query.Filter.CellLink.Value)
                {
                    case CellLink.None:
                        queryable = queryable.Where(x => !x.Reports.Any(r => r.Date >= sincePeriod) &&
                                                        !x.CellsLeading.Any() &&
                                                        !x.CellsServing.Any() &&
                                                        !x.CellsHosting.Any() &&
                                                        (x.TotalCells == 0 || !x.TotalCells.HasValue));
                        break;

                    case CellLink.Attendee:
                        queryable = queryable.Where(x => x.Reports.Any(r => r.Date >= sincePeriod));
                        break;

                    case CellLink.Leader:
                        queryable = queryable.Where(x => x.CellsLeading.Any());
                        break;

                    case CellLink.Timothy:
                        queryable = queryable.Where(x => x.CellsServing.Any());
                        break;

                    case CellLink.Host:
                        queryable = queryable.Where(x => x.CellsHosting.Any());
                        break;
                }
            }

            return queryable;
        }

        public static IQueryable<Member> ApplyMinistryFilter(this IQueryable<Member> queryable, ClaimsPrincipal user)
        {
            if (user == null)
                return queryable;

            if (user.IsSuperAdmin())
                return queryable;

            var memberIdClaim = user.FindFirstValue(CustomClaims.MemberId);
            if (!int.TryParse(memberIdClaim, out var memberId))
                return queryable;

            var spouseIdClaim = user.FindFirstValue(CustomClaims.SpouseId);
            _ = int.TryParse(spouseIdClaim, out var spouseId);

            var g12IdClaim = user.FindFirstValue(CustomClaims.G12LeadershipId);
            _ = int.TryParse(g12IdClaim, out var g12Id);

            var g12SpouseIdClaim = user.FindFirstValue(CustomClaims.G12LeadershipSpouseId);
            _ = int.TryParse(g12SpouseIdClaim, out var g12SpouseId);

            queryable = queryable.Where(x =>
            x.LeadershipIdTree.Contains($"|{memberId}|") ||
            (spouseId > 0 ? x.LeadershipIdTree.Contains($"|{spouseId}|") : false) ||
            (g12Id > 0 ? x.LeadershipIdTree.Contains($"|{g12Id}|") : false) ||
            (g12SpouseId > 0 ? x.LeadershipIdTree.Contains($"|{g12SpouseId}|") : false) ||
            x.Id.Equals(memberId) || x.Id.Equals(spouseId) ||
            x.Id.Equals(g12Id) || x.Id.Equals(g12SpouseId));

            return queryable;
        }

        #endregion

        #region Cell

        public static IQueryable<Cell> ApplyFilter(this IQueryable<Cell> queryable, Query<CellFilter> query)
        {
            if (query == null)
                return queryable;

            if (query.Filter.LeadId > 0)
                queryable = queryable.Where(x => x.LeadId.Equals(query.Filter.LeadId));

            if (!string.IsNullOrWhiteSpace(query.Filter.Lead))
                queryable = queryable.Where(x => x.Lead.Name.ToLower().Contains(query.Filter.Lead));

            if (!string.IsNullOrWhiteSpace(query.Filter.Leadership))
                queryable = queryable.Where(x => x.Lead.ComputedG12Leader.ToLower().Contains(query.Filter.Leadership));

            if (query.Filter.HostId > 0)
                queryable = queryable.Where(x => x.HostId.Equals(query.Filter.HostId));

            if (query.Filter.TimothyId > 0)
                queryable = queryable.Where(x => x.TimothyId.Equals(query.Filter.TimothyId));

            if (query.Filter.WeekDay.HasValue)
                queryable = queryable.Where(x => x.WeekDay.Equals(query.Filter.WeekDay));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_Neighborhood))
                queryable = queryable.Where(x => x.Place.Neighborhood.ToLower().Contains(query.Filter.HomeAddress_Neighborhood.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_City))
                queryable = queryable.Where(x => x.Place.City.ToLower().Contains(query.Filter.HomeAddress_City.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_State))
                queryable = queryable.Where(x => x.Place.State.ToLower().Contains(query.Filter.HomeAddress_State.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_Street))
                queryable = queryable.Where(x => x.Place.Street.ToLower().Contains(query.Filter.HomeAddress_Street.ToLower()));

            if (query.Filter.CellType.HasValue)
                queryable = queryable.Where(x => x.CellType.Equals(query.Filter.CellType));

            if (query.Filter.LeadIdAllLevels.HasValue)
                queryable = queryable.Where(x => x.Lead.LeadershipIdTree.Contains($"|{query.Filter.LeadIdAllLevels}|") ||
                                x.LeadId.Equals(query.Filter.LeadIdAllLevels));

            if (query.Filter.CreatedDateFrom.HasValue)
                queryable = queryable.Where(x => x.CreationDate >= query.Filter.CreatedDateFrom.Value);

            if (query.Filter.CreatedDateUntil.HasValue)
                queryable = queryable.Where(x => x.CreationDate <= query.Filter.CreatedDateUntil.Value);

            if (query.Filter.ReportStatus.HasValue)
            {
                var localNow = SystemConstants.BrazilTime.DateTime;

                var weekStart = SystemConstants.BrazilTime.DateTime.GetStartOfWeek();
                var date1week = SystemConstants.BrazilTime.DateTime.GetStartOfWeek().Subtract(TimeSpan.FromDays(7));
                var date2weeks = SystemConstants.BrazilTime.DateTime.GetStartOfWeek().Subtract(TimeSpan.FromDays(14));
                var date3weeks = SystemConstants.BrazilTime.DateTime.GetStartOfWeek().Subtract(TimeSpan.FromDays(21));
                var date4weeks = SystemConstants.BrazilTime.DateTime.GetStartOfWeek().Subtract(TimeSpan.FromDays(28));

                var thisWeekStart = SystemConstants.BrazilTime.Date.GetStartOfWeek();

                switch (query.Filter.ReportStatus)
                {
                    case CellReportStatus.Current:
                        queryable = queryable.Where(x => x.Reports.Where(x => !x.IsDeleted).Any(x => x.Date >= thisWeekStart) ||
                                                        (!x.Reports.Where(x => !x.IsDeleted).Any(x => x.Date >= thisWeekStart) &&
                                                        (
                                                        x.WeekDay > localNow.DayOfWeek ||
                                                            (x.WeekDay == localNow.DayOfWeek &&
                                                            localNow.Hour < (Convert.ToInt32(x.Time.Substring(0, 2)) + 1)
                                                            &&
                                                            localNow.Minute < Convert.ToInt32(x.Time.Substring(3, 2))
                                                        ))));
                        break;

                    case CellReportStatus.NotDoneYet:
                        queryable = queryable.Where(x => !x.Reports.Where(x => !x.IsDeleted).Any(x => x.Date >= thisWeekStart) &&
                                                        (
                                                        x.WeekDay > localNow.DayOfWeek ||
                                                            (x.WeekDay == localNow.DayOfWeek &&
                                                            localNow.Hour < (Convert.ToInt32(x.Time.Substring(0, 2)) + 1)
                                                            &&
                                                            localNow.Minute < Convert.ToInt32(x.Time.Substring(3, 2))
                                                        )));
                        break;

                    case CellReportStatus.OverDueThisWeek:
                        queryable = queryable.Where(x => !x.Reports.Where(x => !x.IsDeleted).Any(x => x.Date >= thisWeekStart) &&
                                                        (
                                                            x.WeekDay < localNow.DayOfWeek ||
                                                            (x.WeekDay == localNow.DayOfWeek &&
                                                            localNow.Hour > (Convert.ToInt32(x.Time.Substring(0, 2)) + 1)
                                                            &&
                                                            localNow.Minute > Convert.ToInt32(x.Time.Substring(3, 2)))));
                        break;

                    case CellReportStatus.OverDue1Week:
                        queryable = queryable.Where(x =>
                            x.Reports.Where(x => !x.IsDeleted).OrderBy(r => r.Date).Select(r => r.Date).LastOrDefault() > date2weeks &&
                            x.Reports.Where(x => !x.IsDeleted).OrderBy(r => r.Date).Select(r => r.Date).LastOrDefault() < date1week);
                        break;

                    case CellReportStatus.OverDue2Week:
                        queryable = queryable.Where(x =>
                            x.Reports.Where(x => !x.IsDeleted).OrderBy(r => r.Date).Select(r => r.Date).LastOrDefault() > date3weeks &&
                            x.Reports.Where(x => !x.IsDeleted).OrderBy(r => r.Date).Select(r => r.Date).LastOrDefault() < date2weeks);
                        break;

                    case CellReportStatus.OverDue3Week:
                        queryable = queryable.Where(x =>
                            x.Reports.Where(x => !x.IsDeleted).OrderBy(r => r.Date).Select(r => r.Date).LastOrDefault() > date4weeks &&
                            x.Reports.Where(x => !x.IsDeleted).OrderBy(r => r.Date).Select(r => r.Date).LastOrDefault() < date3weeks);
                        break;

                    case CellReportStatus.OverDue4Week:
                        queryable = queryable.Where(x =>
                            x.Reports.Where(x => !x.IsDeleted).OrderBy(r => r.Date).Select(r => r.Date).LastOrDefault() < date4weeks);
                        break;
                }

            }

            return queryable;
        }

        public static IQueryable<Cell> ApplyMinistryFilter(this IQueryable<Cell> queryable, ClaimsPrincipal user)
        {
            if (user == null)
                return queryable;

            if (user.IsSuperAdmin())
                return queryable;

            var memberIdClaim = user.FindFirstValue(CustomClaims.MemberId);
            if (!int.TryParse(memberIdClaim, out var memberId))
                return queryable;

            var spouseIdClaim = user.FindFirstValue(CustomClaims.SpouseId);
            _ = int.TryParse(spouseIdClaim, out var spouseId);

            var g12IdClaim = user.FindFirstValue(CustomClaims.G12LeadershipId);
            _ = int.TryParse(g12IdClaim, out var g12Id);

            var g12SpouseIdClaim = user.FindFirstValue(CustomClaims.G12LeadershipSpouseId);
            _ = int.TryParse(g12SpouseIdClaim, out var g12SpouseId);

            queryable = queryable.Where(x =>
                x.Lead.LeadershipIdTree.Contains($"|{memberId}|") ||
                x.LeadId.Equals(memberId) ||
                x.Host.LeadershipIdTree.Contains($"|{memberId}|") ||
                x.HostId.Equals(memberId) ||
                x.Timothy.LeadershipIdTree.Contains($"|{memberId}|") ||
                x.TimothyId.Equals(memberId) ||

                x.Lead.LeadershipIdTree.Contains($"|{spouseId}|") ||
                x.LeadId.Equals(spouseId) ||
                x.Host.LeadershipIdTree.Contains($"|{spouseId}|") ||
                x.HostId.Equals(spouseId) ||
                x.Timothy.LeadershipIdTree.Contains($"|{spouseId}|") ||
                x.TimothyId.Equals(spouseId) ||

                x.Lead.LeadershipIdTree.Contains($"|{g12Id}|") ||
                x.LeadId.Equals(g12Id) ||
                x.Host.LeadershipIdTree.Contains($"|{g12Id}|") ||
                x.HostId.Equals(g12Id) ||
                x.Timothy.LeadershipIdTree.Contains($"|{g12Id}|") ||
                x.TimothyId.Equals(g12Id) ||

                x.Lead.LeadershipIdTree.Contains($"|{g12SpouseId}|") ||
                x.LeadId.Equals(g12SpouseId) ||
                x.Host.LeadershipIdTree.Contains($"|{g12SpouseId}|") ||
                x.HostId.Equals(g12SpouseId) ||
                x.Timothy.LeadershipIdTree.Contains($"|{g12SpouseId}|") ||
                x.TimothyId.Equals(g12SpouseId));

            return queryable;
        }

        #endregion

        #region Report

        public static IQueryable<Report> ApplyFilter(this IQueryable<Report> queryable, Query<ReportFilter> query)
        {
            if (query == null)
                return queryable;

            if (!string.IsNullOrWhiteSpace(query.Filter.Subject))
                queryable = queryable.Where(x => x.Subject.ToLower().Contains(query.Filter.Subject.ToLower()));

            if (query.Filter.Date.HasValue)
                queryable = queryable.Where(x => x.Date.Equals(query.Filter.Date));

            if (query.Filter.StartDate.HasValue)
                queryable = queryable.Where(x => x.Date >= query.Filter.StartDate.Value);

            if (query.Filter.EndDate.HasValue)
                queryable = queryable.Where(x => x.Date <= query.Filter.EndDate.Value);

            if (query.Filter.CellType.HasValue)
                queryable = queryable.Where(x => x.CellType.Equals(query.Filter.CellType));

            if (query.Filter.LeadershipId > 0)
                queryable = queryable.Where(x => x.LeadershipId.Equals(query.Filter.LeadershipId));

            if (query.Filter.HostId > 0)
                queryable = queryable.Where(x => x.HostId.Equals(query.Filter.HostId));

            if (query.Filter.TimothyId > 0)
                queryable = queryable.Where(x => x.TimothyId.Equals(query.Filter.TimothyId));

            if (query.Filter.CellId > 0)
                queryable = queryable.Where(x => x.CellId.Equals(query.Filter.CellId));

            if (query.Filter.WeekCellSkipped.HasValue)
                queryable = queryable.Where(x => x.WeekCellSkipped.Equals(query.Filter.WeekCellSkipped.Value));

            return queryable;
        }

        public static IQueryable<Report> ApplyMinistryFilter(this IQueryable<Report> queryable, ClaimsPrincipal user)
        {
            if (user == null)
                return queryable;

            if (user.IsSuperAdmin())
                return queryable;

            var memberIdClaim = user.FindFirstValue(CustomClaims.MemberId);
            if (!int.TryParse(memberIdClaim, out var memberId))
                return queryable;

            var spouseIdClaim = user.FindFirstValue(CustomClaims.SpouseId);
            _ = int.TryParse(spouseIdClaim, out var spouseId);

            var g12IdClaim = user.FindFirstValue(CustomClaims.G12LeadershipId);
            _ = int.TryParse(g12IdClaim, out var g12Id);

            var g12SpouseIdClaim = user.FindFirstValue(CustomClaims.G12LeadershipSpouseId);
            _ = int.TryParse(g12SpouseIdClaim, out var g12SpouseId);

            queryable = queryable.Where(x =>
                x.Lead.LeadershipIdTree.Contains($"|{memberId}|") ||
                x.LeadershipId.Equals(memberId) ||
                x.Host.LeadershipIdTree.Contains($"|{memberId}|") ||
                x.HostId.Equals(memberId) ||
                x.Timothy.LeadershipIdTree.Contains($"|{memberId}|") ||
                x.TimothyId.Equals(memberId) ||

                x.Lead.LeadershipIdTree.Contains($"|{spouseId}|") ||
                x.LeadershipId.Equals(spouseId) ||
                x.Host.LeadershipIdTree.Contains($"|{spouseId}|") ||
                x.HostId.Equals(spouseId) ||
                x.Timothy.LeadershipIdTree.Contains($"|{spouseId}|") ||
                x.TimothyId.Equals(spouseId) ||

                x.Lead.LeadershipIdTree.Contains($"|{g12Id}|") ||
                x.LeadershipId.Equals(g12Id) ||
                x.Host.LeadershipIdTree.Contains($"|{g12Id}|") ||
                x.HostId.Equals(g12Id) ||
                x.Timothy.LeadershipIdTree.Contains($"|{g12Id}|") ||
                x.TimothyId.Equals(g12Id) ||

                x.Lead.LeadershipIdTree.Contains($"|{g12SpouseId}|") ||
                x.LeadershipId.Equals(g12SpouseId) ||
                x.Host.LeadershipIdTree.Contains($"|{g12SpouseId}|") ||
                x.HostId.Equals(g12SpouseId) ||
                x.Timothy.LeadershipIdTree.Contains($"|{g12SpouseId}|") ||
                x.TimothyId.Equals(g12SpouseId));

            return queryable;
        }

        #endregion
    }
}
