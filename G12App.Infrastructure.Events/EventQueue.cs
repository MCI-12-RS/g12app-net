using G12App.Domain.Interfaces.Events;
using G12App.Infrastructure.Events.Interfaces;
using System.Collections.Concurrent;

namespace G12App.Infrastructure.Events
{
    public class EventQueue : IEventQueue
    {
        private readonly ConcurrentQueue<IEvent> _events = new ConcurrentQueue<IEvent>();

        public void AddEvent(IEvent ev)
        {
            _events.Enqueue(ev);
        }

        public IEvent Dequeue()
        {
            _events.TryDequeue(out var ev);
            return ev;
        }
    }
}
