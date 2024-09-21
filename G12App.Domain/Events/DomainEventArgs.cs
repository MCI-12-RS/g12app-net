using G12App.Domain.Interfaces.Events;
using System;

namespace G12App.Domain.Events
{
    public record DomainEventArgs
        (IServiceProvider ServiceProvider, IEvent Event);
}
