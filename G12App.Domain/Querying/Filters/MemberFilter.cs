namespace G12App.Domain.Querying.QueryModels
{
    public class MemberFilter : BaseQueryFilter
    {
        public string Name { get; set; }

        public string LeadershipName { get; set; }

        public string Email { get; set; }

        public string HomeAddress_Neighborhood { get; set; }

        public string HomeAddress_City { get; set; }

        public string HomeAddress_State { get; set; }
    }
}
