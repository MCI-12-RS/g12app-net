namespace G12App.Domain.Interfaces.Events
{
    public interface IEventDispatcher
    {
        void DispatchEvent<TEvent>() where TEvent : class, IEvent, new();

        void DispatchEvent<TEvent>(TEvent ev) where TEvent : IEvent;
    }
}
