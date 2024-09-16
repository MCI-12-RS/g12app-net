using G12App.Domain.DomainEvents;

namespace G12App.Domain.Interfaces.EventHandlers
{
    public interface IMemberChangedHandler : IEventHandler<MemberChanged>
    {
    }
}
