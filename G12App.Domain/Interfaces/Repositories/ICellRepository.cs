using G12App.Domain.Model;
using G12App.Domain.Querying;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Repositories
{
    public interface ICellRepository
    {
        Task<IEnumerable<Cell>> GetCellssAsync(Query query);
        Task<Cell> GetCellByIdAsync(int? id);
        Task<Cell> Create(Cell cell);
        Task<Cell> Update(Cell cell);
        Task<Cell> Remove(Cell cell);
    }   
}
