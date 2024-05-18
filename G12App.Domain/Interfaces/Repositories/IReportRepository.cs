using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetReportsAsync(Query<ReportFilter> query);
        Task<int> CountReportsAsync(Query<ReportFilter> query);
        Task<Report> GetReportById(int Id);
        Task<Report> Create(Report report);
        Task<Report> Update(Report report);
        Task<Report> Remove(Report report);
    }
}
