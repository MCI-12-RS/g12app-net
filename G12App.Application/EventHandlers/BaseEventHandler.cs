using G12App.Domain.Interfaces.EventHandlers;
using G12App.Domain.Interfaces.Events;
using Microsoft.Extensions.Logging;

namespace G12App.Application.EventHandlers
{
    public class BaseEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        private readonly ILogger<BaseEventHandler<TEvent>> _logger;

        public BaseEventHandler(ILogger<BaseEventHandler<TEvent>> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TEvent ev)
        {
            _logger.LogInformation("Handling event {eventName} for {entityId}", ev.GetType().Name, ev.EntityId);

            await HandleInternal(ev);
        }

        protected virtual Task HandleInternal(TEvent ev)
            => Task.CompletedTask;

        public Task Handle(IEvent ev)
            => Handle((TEvent)ev);
    }
}
