using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.Querying.Filters
{
    public class MemberFilter : BaseQueryFilter
    {
        public string Name { get; set; }

        public string LeadershipName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string HomeAddress_Neighborhood { get; set; }

        public string HomeAddress_City { get; set; }

        public string HomeAddress_State { get; set; }

        public int? MinistryLevel { get; set; }

        public int[] MinistryLevels { get; set; }

        public int[] NotInId { get; set; }

        public int? LeadershipId { get; set; }

        public bool? ChildrenMembersComputed { get; set; }

        public int? LeadershipIdAllLevels { get; set; }

        public DateTime? CreatedDateFrom { get; set; }

        public DateTime? CreatedDateUntil { get; set; }

        public bool? Baptized { get; set; }

        public bool? Idle { get; set; }

        public bool? HasNoLeadership { get; set; }

        public CellLink? CellLink { get; set; }
    }
}
