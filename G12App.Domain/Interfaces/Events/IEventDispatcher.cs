using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Events
{
    public interface IEventDispatcher
    {
        ValueTask DispatchEvent<TEvent>() where TEvent : class, IEvent, new();

        ValueTask DispatchEvent<TEvent>(TEvent ev) where TEvent : IEvent;
    }
}
