namespace G12App.Application.Events
{
    public class MemberUpserted : BaseEvent
    {
        private readonly int _memberId;

        public MemberUpserted(int memberId)
            => (_memberId) = (memberId);
    }
}
