using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class MemberCreated : MemberChanged
    {
        public MemberCreated(MemberDto member)
            : base(null, member)
        {
        }
    }
}
