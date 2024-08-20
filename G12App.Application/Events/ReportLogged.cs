using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class ReportLogged : BaseEvent
    {
        private readonly ReportDto _before;
        private readonly ReportDto _after;

        protected override object EntityId => _after?.Id ?? _before?.Id;

        public ReportLogged(ReportDto before, ReportDto after)
            => (_before, _after) = (before, after);
    }
}
