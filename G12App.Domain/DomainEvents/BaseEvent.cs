using G12App.Domain.Interfaces.Events;

namespace G12App.Domain.DomainEvents
{
    public abstract class BaseEvent : IEvent
    {
        public abstract object EntityId { get; }

        public override string ToString()
        {
            return string.Concat(this.GetType().Name, ": ", EntityId);
        }
    }
}
