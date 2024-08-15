using G12App.Domain.Interfaces.Events;
using G12App.Infrastructure.Events.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace G12App.Infrastructure.Events
{
    public class EventWorker : BackgroundService
    {
        private readonly IEventQueue _eventQueue;
        private readonly ILogger<EventWorker> _logger;

        public EventWorker(
            IEventQueue eventQueue,
            ILogger<EventWorker> logger)
        {
            _eventQueue = eventQueue;
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

                    await ev.Handle(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Exception occurred while processing an event {type}.", ev?.GetType());
                }
            }
        }
    }
}
