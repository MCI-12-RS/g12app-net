using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Domain.Querying
{
    public class Query<TFilter> : Query
        where TFilter : BaseQueryFilter  
    {
        public TFilter Filter { get; set; }
    }

    public class Query 
    {        
        public Query()
        {
            Pagination = new Pagination();
            Sort = new Sort();
        }

        public Query(int page, int limit, string sortBy, string sortOrder)
        {
            Pagination = new Pagination(page, limit);
            Sort = new Sort(sortBy, sortOrder);
        }

        public Pagination Pagination { get; set; }

        public Sort Sort { get; set; }
    }
}