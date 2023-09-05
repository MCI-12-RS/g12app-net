using G12App.Domain.Querying;

namespace G12App.Common.Extensions
{
    public static class QueryingExtensions
    {
        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> queryable, Query query)
        {
            return queryable.Skip((query.Pagination.Page -1) * query.Pagination.Limit).Take(query.Pagination.Limit);
        }
    }
}
