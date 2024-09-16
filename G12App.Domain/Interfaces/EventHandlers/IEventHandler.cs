using G12App.Domain.Interfaces.Events;
using System.Threading;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.EventHandlers
{
    public interface IEventHandler<TEvent>
        where TEvent : IEvent
    {
        Task Handle(TEvent ev);
    }
}
