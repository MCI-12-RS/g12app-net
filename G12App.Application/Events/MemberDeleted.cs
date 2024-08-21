using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class MemberDeleted : MemberChanged
    {
        public MemberDeleted(MemberDto member)
            : base(member, null)
        {
        }
    }
}
