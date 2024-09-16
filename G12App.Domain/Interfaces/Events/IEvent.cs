using System;
using System.Threading;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Events
{
    public interface IEvent
    {
        object EntityId { get; }
    }
}
