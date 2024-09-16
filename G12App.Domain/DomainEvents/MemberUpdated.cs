using G12App.Domain.DTOs;

namespace G12App.Domain.DomainEvents
{
    public class MemberUpdated : MemberChanged
    {
        public MemberUpdated(MemberDto before, MemberDto after)
            : base(before, after)
        {
        }
    }
}
