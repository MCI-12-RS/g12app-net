using G12App.Domain.Model.Base;
using G12App.Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace G12App.Domain.Model
{
    public class Member : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }

        public Gender Gender { get; set; }

        public string Email { get; set; }

        public Address HomeAddress { get; set; }

        public int? HomeAddressId { get; set; }

        public Member Leadership { get; set; }

        public int? LeadershipId { get; set; }

        public int? MinistryLevel { get; set; }

        public ICollection<Report> Reports { get; set; }
    }
}