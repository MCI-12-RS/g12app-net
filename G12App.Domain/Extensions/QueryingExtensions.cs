using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.QueryModels;
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
                queryable = queryable.Where(x => x.Name.Contains(query.Filter.Name));

            if (!string.IsNullOrWhiteSpace(query.Filter.LeadershipName))
                queryable = queryable.Where(x => x.Leadership.Name.Contains(query.Filter.LeadershipName));

            if (!string.IsNullOrWhiteSpace(query.Filter.Email))
                queryable = queryable.Where(x => x.Email.Contains(query.Filter.Email));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_Neighborhood))
                queryable = queryable.Where(x => x.HomeAddress.Neighborhood.Contains(query.Filter.HomeAddress_Neighborhood));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_City))
                queryable = queryable.Where(x => x.HomeAddress.City.Contains(query.Filter.HomeAddress_City));

            if (!string.IsNullOrWhiteSpace(query.Filter.HomeAddress_State))
                queryable = queryable.Where(x => x.HomeAddress.State.Contains(query.Filter.HomeAddress_State));

            return queryable;
        }
    }
}
