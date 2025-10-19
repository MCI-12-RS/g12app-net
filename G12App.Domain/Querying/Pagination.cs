namespace G12App.Domain.Querying
{
    public class Pagination
    {
        public Pagination()
        {
        }

        public Pagination(int page, int limit)
        {
            Page = page;
            Limit = limit;
        }

        public int Page { get; set; }

        public int Limit { get; set; }

        public static Pagination SingleRecord => new Pagination(1, 1);
    }
}
