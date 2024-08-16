namespace G12App.Application.Events
{
    public class ReportLogged : BaseEvent
    {
        private readonly int _cellId;
        private readonly int _reportId;

        public ReportLogged(int reportId, int cellId)
            => (_reportId, _cellId) = (reportId, cellId);
    }
}
