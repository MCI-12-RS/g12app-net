using G12App.Domain.DTOs;

namespace G12App.Domain.DomainEvents
{
    public class MemberCreated : MemberChanged
    {
        public MemberCreated(MemberDto member)
            : base(null, member)
        {
        }
    }
}
