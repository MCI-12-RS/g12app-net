using G12App.Domain.Interfaces.Events;

namespace G12App.Infrastructure.Events
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IEventQueue _eventQueue;

        public EventDispatcher(IEventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        public ValueTask DispatchEvent<TEvent>(TEvent ev) where TEvent : IEvent
            => _eventQueue.AddEvent(ev, CancellationToken.None);

        public ValueTask DispatchEvent<TEvent>()
            where TEvent : class, IEvent, new()
                => this.DispatchEvent(new TEvent());
    }
}
