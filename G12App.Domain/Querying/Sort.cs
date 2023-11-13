namespace G12App.Domain.Querying
{
    public class Sort
    {
        public Sort()
        {
        }

        public Sort(string sortBy, bool order)
        {
            SortBy = sortBy;
            Order = order;
        }

        public string SortBy { get; set; }

        public bool Order { get; set; }
    }
}