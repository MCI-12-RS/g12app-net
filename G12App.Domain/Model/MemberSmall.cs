using G12App.Domain.Model.Base;
using G12App.Domain.Model.Enum;

namespace G12App.Domain.Model
{
    public class MemberSmall : BaseEntity
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int? LeadershipId { get; set; }
        public int? MinistryLevel { get; set; }
        public int? SpouseId { get; set; }
        public string ComputedG12Leader { get; set; }
        public string ComputedLeadershipName { get; set; }
        public int? TotalDisciples { get; set; }
        public int? TotalCells { get; set; }
        public int CellsLeadingCount { get; set; }
        public int CellsHostingCount { get; set; }
        public int CellsServingCount { get; set; }
        public MemberSmall Spouse { get; set; }
        public bool Baptized { get; set; }
    }
}
