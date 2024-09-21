using G12App.Domain.Interfaces.EventHandlers;
using G12App.Domain.Interfaces.Events;
using Microsoft.Extensions.DependencyInjection;

namespace G12App.Infrastructure.Events
{
    public class EventHandlerService : IEventHandlerService
    {
        private readonly IServiceProvider _serviceProvider;

        public EventHandlerService(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task<List<IEventHandler>> GetHandlers(Type eventType)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
            await using var scope = _serviceProvider.CreateAsyncScope();
            var services = scope.ServiceProvider.GetServices(handlerType);
            return services.Select(x => x as IEventHandler).ToList();
        }
    }
}
