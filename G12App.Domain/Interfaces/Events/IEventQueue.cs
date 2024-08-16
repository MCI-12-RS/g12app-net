using System.Threading;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Events
{
    public interface IEventQueue
    {
        ValueTask AddEvent(IEvent ev, CancellationToken token);

        ValueTask<IEvent> Dequeue(CancellationToken token);
    }
}
