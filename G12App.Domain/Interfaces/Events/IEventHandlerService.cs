using G12App.Domain.Interfaces.EventHandlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Events
{
    public interface IEventHandlerService
    {
        Task<List<IEventHandler>> GetHandlers(Type eventType);
    }
}
