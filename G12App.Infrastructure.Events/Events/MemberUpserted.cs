using G12App.Domain.Interfaces.Events;

namespace G12App.Infrastructure.Events.Events
{
    public class MemberUpserted : IEvent
    {
        private readonly int _memberId;

        public MemberUpserted(int memberId)
        {
            _memberId = memberId;
        }

        public Task Handle(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
