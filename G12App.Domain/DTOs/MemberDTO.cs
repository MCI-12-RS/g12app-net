using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.DTOs
{
    public class MemberDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public AddressDto HomeAddress { get; set; }
        public int? LeadershipId { get; set; }
        public MemberDto Leadership { get; set; }
        public int? MinistryLevel { get; set; }
        public int? SpouseId { get; set; }
        public string ComputedG12Leader { get; set; }
        public string ComputedLeadershipName { get; set; }
        public string LeadershipTree { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
