using G12App.Domain.Interfaces.Events;
using Microsoft.Extensions.DependencyInjection;

namespace G12App.Infrastructure.Events
{
    public static class Startup
    {
        public static IServiceCollection RegisterEvents(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddSingleton<IEventDispatcher, EventDispatcher>();
            servicesCollection.AddSingleton<IEventQueue, ChannelEventQueue>();

            servicesCollection.AddHostedService<EventWorker>();

            return servicesCollection;
        }
    }
}
