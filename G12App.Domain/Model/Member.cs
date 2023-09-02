using G12App.Domain.Model.Enum;

namespace G12App.Domain.Model
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MemberType MemberType { get; set; }
        public int BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public Address HomeAddress { get; set; }
        public Member Leadership { get; set; }
    }
}