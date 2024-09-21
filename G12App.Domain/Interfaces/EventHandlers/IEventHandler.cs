using G12App.Domain.Interfaces.Events;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.EventHandlers
{
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : IEvent
    {
        Task Handle(TEvent ev);
    }

    public interface IEventHandler
    {
        Task Handle(IEvent ev);
    }
}
