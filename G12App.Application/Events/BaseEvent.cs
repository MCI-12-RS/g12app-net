using G12App.Domain.Interfaces.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace G12App.Application.Events
{
    public abstract class BaseEvent : IEvent
    {
        protected ILogger Logger;
        protected abstract object EntityId { get; }

        public async Task Handle(IServiceProvider serviceProvider, CancellationToken token)
        {
            Logger = serviceProvider.GetRequiredService<ILogger<BaseEvent>>();

            Logger.LogInformation("Handling event {eventName} for {entityId}", this.GetType().Name, EntityId);

            await HandleInternal(serviceProvider, token);
        }

        protected virtual Task HandleInternal(IServiceProvider serviceProvider, CancellationToken token)
            => Task.CompletedTask;
    }
}
