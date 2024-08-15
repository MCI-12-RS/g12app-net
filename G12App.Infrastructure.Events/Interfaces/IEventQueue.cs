using G12App.Domain.Interfaces.Events;

namespace G12App.Infrastructure.Events.Interfaces
{
    public interface IEventQueue
    {
        ValueTask AddEvent(IEvent ev, CancellationToken token);

        ValueTask<IEvent> Dequeue(CancellationToken token);
    }
}
