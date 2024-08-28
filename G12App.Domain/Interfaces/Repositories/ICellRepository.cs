using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Repositories
{
    public interface ICellRepository
    {
        Task<IEnumerable<Cell>> GetCellsAsync(Query<CellFilter> query);
        Task<int> CountCellsAsync(Query<CellFilter> query);
        Task<Cell> GetCellByIdAsync(int? id);
        Task<Cell> Create(Cell cell);
        Task<Cell> Update(Cell cell);
        Task<Cell> Remove(Cell cell);
        IQueryable<Cell> GetCellsPendingReport(Query<CellFilter> cellQuery, Query<ReportFilter> reportQuery);
    }
}
