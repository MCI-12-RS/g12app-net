using G12App.Domain.Model;
using G12App.Domain.Querying;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Repositories
{
    public  interface IReportRepository
    {
        Task<IEnumerable<Report>> GetReportsAsync(Query query);
        Task<IEnumerable<Report>> GetByLeadId(Query query, int leadId);
        Task<IEnumerable<Report>> GetByCellId(Query query, int leadId);
        Task<Report> GetReportById(int reportId);
        Task<Report> Create (Report report);
        Task<Report> Update (Report report);
        Task<Report> Remove (Report report);
    }
}
