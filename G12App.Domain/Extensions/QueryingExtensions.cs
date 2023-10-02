using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System;
using System.Linq;

namespace G12App.Domain.Extensions
{
    public static class QueryingExtensions
    {
        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> queryable, Query query)
        {
            return queryable.Skip((query.Pagination.Page - 1) * query.Pagination.Limit).Take(query.Pagination.Limit);
        }

        public static IQueryable<Member> ApplyFilter(this IQueryable<Member> queryable, Query<MemberFilter> query)
        {
            if (!string.IsNullOrWhiteSpace(query.Filter.Name))
                queryable = queryable.Where(x => x.Name.Contains(query.Filter.Name,StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrWhiteSpace(query.Filter.LeadershipName))
                queryable = queryable.Where(x => x.Leadership.Name.Contains(query.Filter.LeadershipName, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrWhiteSpace(query.Filter.Email))
                queryable = queryable.Where(x => x.Email.Contains(query.Filter.Email, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_Neighborhood))
                queryable = queryable.Where(x => x.HomeAddress.Neighborhood.Contains(query.Filter.HomeAddress_Neighborhood, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_City))
                queryable = queryable.Where(x => x.HomeAddress.City.Contains(query.Filter.HomeAddress_City, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_State))
                queryable = queryable.Where(x => x.HomeAddress.State.Contains(query.Filter.HomeAddress_State, StringComparison.InvariantCultureIgnoreCase));

            return queryable;
        }

        public static IQueryable<Cell> ApplyFilter(this IQueryable<Cell> queryable, Query<CellFilter> query)
        {
            if (query.Filter.LeadId>0)
                queryable = queryable.Where(x => x.LeadId.Equals(query.Filter.LeadId));

            if (query.Filter.HostId>0)
                queryable = queryable.Where(x => x.HostId.Equals(query.Filter.HostId));

            if (query.Filter.TimothyId>0)
                queryable = queryable.Where(x => x.TimothyId.Equals(query.Filter.TimothyId));

            if (query.Filter.WeekDay.HasValue)
                queryable = queryable.Where(x => x.WeekDay.Equals(query.Filter.WeekDay));

            if (query.Filter.PlaceId > 0)
                queryable = queryable.Where(x => x.PlaceId.Equals(query.Filter.PlaceId));

            if (query.Filter.CellType.HasValue)
                queryable = queryable.Where(x => x.CellType.Equals(query.Filter.CellType));

            return queryable;
        }

        public static IQueryable<Report> ApplyFilter(this IQueryable<Report> queryable, Query<ReportFilter> query)
        {
            if (!string.IsNullOrWhiteSpace(query.Filter.Subject))
                queryable = queryable.Where(x => x.Subject.Contains(query.Filter.Subject, StringComparison.InvariantCultureIgnoreCase));

            if (query.Filter.Date.HasValue)
                queryable = queryable.Where(x => x.Date.Equals(query.Filter.Date));

            if (query.Filter.CellType.HasValue)
                queryable = queryable.Where(x => x.CellType.Equals(query.Filter.CellType));

            if (query.Filter.LeadershipId > 0)
                queryable = queryable.Where(x => x.LeadershipId.Equals(query.Filter.LeadershipId));

            if (query.Filter.HostId > 0)
                queryable = queryable.Where(x => x.HostId.Equals(query.Filter.HostId));

            if (query.Filter.TimothyId > 0)
                queryable = queryable.Where(x => x.TimothyId.Equals(query.Filter.TimothyId));

            return queryable;
        }
    }
}
