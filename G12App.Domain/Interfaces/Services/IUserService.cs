using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<ServiceResult<IEnumerable<UserDTO>>> GetUsers(Query<UserFilter> query);
        Task<ServiceResult<UserDTO>> GetUserById(int? id);
        Task<ServiceResult<UserDTO>> Add(UserDTO userDTO);
        Task<ServiceResult<UserDTO>> Update(UserDTO userDTO, int id);
        Task<ServiceResult> Remove(int? id);
    }
}
