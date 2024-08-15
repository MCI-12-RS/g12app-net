using G12App.Infrastructure.Events.Interfaces;
using Microsoft.Extensions.Hosting;

namespace G12App.Infrastructure.Events
{
    public class EventWorker : BackgroundService
    {
        private readonly IEventQueue _eventQueue;

        public EventWorker(IEventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var ev = _eventQueue.Dequeue();
                if (ev == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    continue;
                }

                await ev.Handle(stoppingToken);
            }
        }
    }
}
