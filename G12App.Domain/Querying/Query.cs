namespace G12App.Domain.Querying
{
    public class Query<TFilter> : Query
        where TFilter : BaseQueryFilter, new()
    {
        public Query()
        {
            Filter = new TFilter();
        }

        public TFilter Filter { get; set; }
    }

    public class Query
    {
        public Query()
        {
            Pagination = new Pagination();
            Sort = new Sort();
        }

        public Query(int page, int limit, string sortBy, bool order)
        {
            Pagination = new Pagination(page, limit);
            Sort = new Sort(sortBy, order);
        }

        public Pagination Pagination { get; set; }

        public Sort Sort { get; set; }
    }
}