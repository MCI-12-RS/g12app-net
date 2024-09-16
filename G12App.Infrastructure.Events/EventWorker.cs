using G12App.Domain.Interfaces.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace G12App.Infrastructure.Events
{
    public class EventWorker : BackgroundService
    {
        private readonly IEventQueue _eventQueue;
        private readonly IEventHandlerRegistry _eventHandlerRegistry;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventWorker> _logger;

        public EventWorker(
            IEventQueue eventQueue,
            IEventHandlerRegistry eventHandlerRegistry,
            IServiceProvider serviceProvider,
            ILogger<EventWorker> logger)
        {
            _eventQueue = eventQueue;
            _eventHandlerRegistry = eventHandlerRegistry;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IEvent ev = null;
                try
                {
                    ev = await _eventQueue.Dequeue(stoppingToken);
                    await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                    if (ev == null)
                        continue;

                    if (!_eventHandlerRegistry.Handlers.TryGetValue(ev.GetType(), out var handler))
                    {
                        _logger.LogWarning("No handler found for event type {type}.", ev.GetType().Name);
                        continue;
                    }

                    await using var scope = _serviceProvider.CreateAsyncScope();
                    await handler(scope.ServiceProvider, ev);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Exception occurred while processing an event {type}.", ev?.GetType());
                }
            }
        }
    }
}
