using G12App.Domain.Model;
using G12App.Domain.Model.Enum;
using System;

namespace G12App.Domain.RequestMessages
{
    public class MemberRequest
    {
        public string Name { get; set; }
        public MemberType MemberType { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public int? LeadershipId { get; set; }
    }
}
