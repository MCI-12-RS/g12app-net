using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class MemberDataConsolidationRequested : MemberChanged
    {
        public MemberDataConsolidationRequested(MemberDto member)
            : base(null, member)
        {
        }
    }
}
