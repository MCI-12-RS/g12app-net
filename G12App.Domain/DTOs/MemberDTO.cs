using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.DTOs
{
    public class MemberDtoSmall
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ComputedG12Leader { get; set; }
        public string ComputedLeadershipName { get; set; }
        public int? SpouseId { get; set; }
    }

    public class MemberDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public AddressDto HomeAddress { get; set; }
        public int? LeadershipId { get; set; }
        public int? MinistryLevel { get; set; }
        public int? SpouseId { get; set; }
        public MemberDtoSmall Leadership { get; set; }
        public MemberDtoSmall Spouse { get; set; }
        public string Notes { get; set; }
        public bool Baptized { get; set; }
        public DateTime? BaptismDate { get; set; }
        public bool HasNoLeadership { get; set; }
        public bool Idle { get; set; }
        public string ComputedG12Leader { get; set; }
        public string ComputedLeadershipName { get; set; }
        public int? ComputedG12LeaderId { get; set; }
        public int? ComputedG12LeaderSpouseId { get; set; }
        public string LeadershipTree { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? TotalDisciples { get; set; }
        public int? TotalCells { get; set; }
    }
}
