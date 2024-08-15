using G12App.Domain.Interfaces.Events;
using G12App.Infrastructure.Events.Interfaces;
using System.Threading.Channels;

namespace G12App.Infrastructure.Events
{
    public class EventQueue : IEventQueue
    {
        private readonly Channel<IEvent> _channel = Channel.CreateUnbounded<IEvent>();
        private ChannelReader<IEvent> Reader => _channel.Reader;
        private ChannelWriter<IEvent> Writer => _channel.Writer;

        public ValueTask AddEvent(IEvent ev, CancellationToken token)
            => Writer.WriteAsync(ev, token);

        public ValueTask<IEvent> Dequeue(CancellationToken token)
            => Reader.ReadAsync();
    }
}
