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
                queryable = queryable.Where(x => x.Name.ToLower().Contains(query.Filter.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.LeadershipName))
                queryable = queryable.Where(x => x.Leadership.Name.ToLower().Contains(query.Filter.LeadershipName.ToLower()));

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

            if (query.Filter.NotInId != null && query.Filter.NotInId.Any())
                queryable = queryable.Where(x => !query.Filter.NotInId.Contains(x.Id));

            return queryable;
        }

        public static IQueryable<Cell> ApplyFilter(this IQueryable<Cell> queryable, Query<CellFilter> query)
        {
            if (query.Filter.LeadId > 0)
                queryable = queryable.Where(x => x.LeadId.Equals(query.Filter.LeadId));

            if (!string.IsNullOrWhiteSpace(query.Filter.Lead))
                queryable = queryable.Where(x => x.Lead.Name.ToLower().Contains(query.Filter.Lead));

            if (!string.IsNullOrWhiteSpace(query.Filter.Leadership))
                queryable = queryable.Where(x => x.Lead.Leadership.Name.ToLower().Contains(query.Filter.Leadership));

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

            return queryable;
        }

        public static IQueryable<Report> ApplyFilter(this IQueryable<Report> queryable, Query<ReportFilter> query)
        {
            if (!string.IsNullOrWhiteSpace(query.Filter.Subject))
                queryable = queryable.Where(x => x.Subject.ToLower().Contains(query.Filter.Subject.ToLower()));

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

        public static IQueryable<User> ApplyFilter(this IQueryable<User> queryable, Query<UserFilter> query)
        {
            if(!string.IsNullOrWhiteSpace(query.Filter.Name))
                queryable = queryable.Where(x => x.Name.ToLower().Contains(query.Filter.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.Filter.Email))
                queryable = queryable.Where(x => x.Email.ToLower().Contains(query.Filter.Email.ToLower()));

            return queryable;
        }
    }
}
