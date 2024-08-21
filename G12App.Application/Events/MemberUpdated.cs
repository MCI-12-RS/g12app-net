using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class MemberUpdated : MemberChanged
    {
        public MemberUpdated(MemberDto before, MemberDto after)
            : base(before, after)
        {
        }
    }
}
