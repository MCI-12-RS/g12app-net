using G12App.Domain.DTOs;
using G12App.Domain.Querying;
using G12App.Domain.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;
using G12App.Domain.Querying.Filters;

namespace G12App.Domain.Interfaces.Services
{
    public interface ICellService
    {
        Task<ServiceResult<IEnumerable<CellDto>>> GetCell(Query<CellFilter> query);
        Task<ServiceResult<CellDto>> GetCellById(int? id);
        Task<ServiceResult<CellDto>> Add(CellDto cellDTO);
        Task<ServiceResult<CellDto>> Update(CellDto cellDTO, int id);
        Task <ServiceResult>Remove(int? id);
    }
}
