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
                    if (ev == null)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                        continue;
                    }

                    await using var scope = _serviceProvider.CreateAsyncScope();
                    await ev.Handle(scope.ServiceProvider, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Exception occurred while processing an event {type}.", ev?.GetType());
                }
            }
        }
    }
}
