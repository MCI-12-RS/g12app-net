using G12App.Domain.Interfaces.Events;
using System.Collections.Concurrent;

namespace G12App.Infrastructure.Events
{
    public class EventQueue : IEventQueue
    {
        private readonly ConcurrentQueue<IEvent> _queue = new ConcurrentQueue<IEvent>();

        public ValueTask AddEvent(IEvent ev, CancellationToken token)
        {
            _queue.Enqueue(ev);
            return ValueTask.CompletedTask;
        }

        public int CountEvent()
        {
            return _queue.Count;
        }

        public ValueTask<IEvent> Dequeue(CancellationToken token)
        {
            IEvent ev = null;
            _queue.TryDequeue(out ev);
            return ValueTask.FromResult(ev);
        }

        public IEvent PeekEvent()
        {
            _queue.TryPeek(out var ev);
            return ev;
        }
    }
}
