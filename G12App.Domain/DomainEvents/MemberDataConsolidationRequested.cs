using G12App.Domain.DTOs;

namespace G12App.Domain.DomainEvents
{
    public class MemberDataConsolidationRequested : MemberChanged
    {
        public MemberDataConsolidationRequested(MemberDto member)
            : base(null, member)
        {
        }
    }
}
