using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class ReportLogged : BaseEvent
    {
        private readonly ReportDTO _before;
        private readonly ReportDTO _after;

        public ReportLogged(ReportDTO before, ReportDTO after)
            => (_before, _after) = (before, after);
    }
}
