using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Domain.Querying
{
    public class Sort
    {
        public Sort()
        {
        }

        public Sort(string sortBy, string sortOrder)
        {
            SortBy = sortBy;
            SortOrder = sortOrder;
        }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }
    }
}