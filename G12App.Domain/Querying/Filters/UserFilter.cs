namespace G12App.Domain.Querying.Filters
{
    public class UserFilter : BaseQueryFilter
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
