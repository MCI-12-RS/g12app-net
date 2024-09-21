using G12App.Domain.Interfaces.EventHandlers;
using G12App.Domain.Interfaces.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace G12App.Infrastructure.Events
{
    public class EventWorker : BackgroundService
    {
        private readonly IEventQueue _eventQueue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventWorker> _logger;

        public EventWorker(
            IEventQueue eventQueue,
            IServiceProvider serviceProvider,
            ILogger<EventWorker> logger)
        {
            _eventQueue = eventQueue;
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

                    var handlerType = typeof(IEventHandler<>).MakeGenericType(ev.GetType());
                    await using var scope = _serviceProvider.CreateAsyncScope();
                    var services = scope.ServiceProvider.GetServices(handlerType).Cast<IEventHandler>();

                    if (services == null || services.Count() == 0)
                    {
                        _logger.LogWarning("No handler found for event type {type}.", ev.GetType().Name);
                        continue;
                    }

                    foreach (var handler in services)
                    {
                        try
                        {
                            await handler.Handle(ev);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogCritical(ex, "Exception occurred while calling handler for event {type}.", ev?.GetType());
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Exception occurred while processing an event {type}.", ev?.GetType());
                }
            }
        }
    }
}
