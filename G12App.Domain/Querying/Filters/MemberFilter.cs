namespace G12App.Domain.Querying.Filters
{
    public class MemberFilter : BaseQueryFilter
    {
        public string Name { get; set; }

        public string LeadershipName { get; set; }

        public string Email { get; set; }

        public string HomeAddress_Neighborhood { get; set; }

        public string HomeAddress_City { get; set; }

        public string HomeAddress_State { get; set; }

        public int? MinistryLevel { get; set; }

        public int[] NotInId { get; set; }

        public int? LeadershipId { get; set; }

        public bool? ChildrenMembersComputed { get; set; }
    }
}
