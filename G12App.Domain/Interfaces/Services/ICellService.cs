using G12App.Domain.DTOs;
using G12App.Domain.Querying;
using G12App.Domain.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface ICellService
    {
        Task<ServiceResult<IEnumerable<CellDTO>>> GetCell(Query query);
        Task<ServiceResult<CellDTO>> GetCellById(int? id);
        Task<ServiceResult<CellDTO>> Add(CellDTO cellDTO);
        Task<ServiceResult<CellDTO>> Update(CellDTO cellDTO, int id);
        Task <ServiceResult>Remove(int? id);
    }
}
