using G12App.Domain.Model.Base;
using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.Model
{
    public class Member : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public MemberType MemberType { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string Email { get; set; }

        public Address HomeAddress { get; set; }

        public int? HomeAddressId { get; set; }

        public Member Leadership { get; set; }

        public int? LeadershipId { get; set; }
    }
}