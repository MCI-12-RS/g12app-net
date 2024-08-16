namespace G12App.Application.Events
{
    public class CellUpserted : BaseEvent
    {
        private readonly int _cellId;

        public CellUpserted(int cellId)
            => (_cellId) = (cellId);
    }
}
