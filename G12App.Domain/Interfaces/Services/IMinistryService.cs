using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IMinistryService
    {
        Task<ServiceResult<IEnumerable<CoupleDto>>> GetCouplesByParentIdAsync(int parentId, int page, int limit);
    }
}
