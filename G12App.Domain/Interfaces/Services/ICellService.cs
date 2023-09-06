using G12App.Domain.DTOs;
using G12App.Domain.Querying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface ICellService
    {
        Task<IEnumerable<CellDTO>> GetMembers(Query query);
        Task<CellDTO> GetCellById(int? id);
        Task<CellDTO> Add(CellDTO cellDTO);
        Task<CellDTO> Update(CellDTO cellDTO, int id);
        Task Remove(int? id);
    }
}
