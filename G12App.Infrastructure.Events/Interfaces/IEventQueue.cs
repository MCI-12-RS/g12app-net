using G12App.Domain.Interfaces.Events;

namespace G12App.Infrastructure.Events.Interfaces
{
    public interface IEventQueue
    {
        void AddEvent(IEvent ev);

        IEvent Dequeue();
    }
}
