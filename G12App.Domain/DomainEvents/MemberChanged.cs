using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Events;

namespace G12App.Domain.DomainEvents
{
    public class MemberChanged : BaseEvent, IEvent
    {
        public MemberDto Before { get; }
        public MemberDto After { get; }

        public override object EntityId => After?.Id ?? Before?.Id;

        public MemberChanged(MemberDto before, MemberDto after)
            => (Before, After) = (before, after);
    }
}
