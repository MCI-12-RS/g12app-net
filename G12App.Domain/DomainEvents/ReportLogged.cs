using G12App.Domain.DTOs;

namespace G12App.Application.Events
{
    public class ReportLogged : ReportChanged
    {
        public ReportLogged(ReportDto after)
            : base(null, after)
        {
        }
    }
}
