using G12App.Domain.DTOs;

namespace G12App.Domain.DomainEvents
{
    public class MemberDeleted : MemberChanged
    {
        public MemberDeleted(MemberDto member)
            : base(member, null)
        {
        }
    }
}
