using G12App.Domain.DomainEvents;
using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class ReportChanged : BaseEvent
    {
        private readonly ReportDto _before;
        private readonly ReportDto _after;

        public override object EntityId => _after?.Id ?? _before?.Id;

        public ReportChanged(ReportDto before, ReportDto after)
            => (_before, _after) = (before, after);
    }
}
