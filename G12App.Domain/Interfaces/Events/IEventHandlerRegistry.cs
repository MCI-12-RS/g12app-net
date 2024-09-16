using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Events
{
    public interface IEventHandlerRegistry
    {
        IReadOnlyDictionary<Type, Func<IServiceProvider, IEvent, Task>> Handlers { get; }
    }
}
