using G12App.Domain.Interfaces.Events;
using G12App.Infrastructure.Events.Interfaces;

namespace G12App.Infrastructure.Events
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IEventQueue _eventQueue;

        public EventDispatcher(IEventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        public void DispatchEvent<TEvent>(TEvent ev) where TEvent : IEvent
        {
            _eventQueue.AddEvent(ev);
        }

        public void DispatchEvent<TEvent>()
            where TEvent : class, IEvent, new()
                => this.DispatchEvent(new TEvent());
    }
}
