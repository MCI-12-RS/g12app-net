using G12App.Domain.Model;
using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.DTOs
{
    public class MemberDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public AddressDto HomeAddress { get; set; }
        public int? LeadershipId { get; set; }
        public MemberDto Leadership { get; set; }
    }
}
