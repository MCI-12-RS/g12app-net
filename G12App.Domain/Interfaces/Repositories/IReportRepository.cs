using G12App.Domain.Model;
using G12App.Domain.Querying;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Repositories
{
    public  interface IReportRepository
    {
        Task<IEnumerable<Report>> GetReportsAsync(Query query);
        Task<IEnumerable<Report>> GetReportsByLeadId(int leadId);
        Task<IEnumerable<Report>> GetByCellId(int? cellId);
        Task<IEnumerable<Report>> GetReportsById(int reportId);
        Task<Report> Create (Report report);
        Task<Report> Update (Report report);
        Task<Report> Remove (Report report);
    }
}
