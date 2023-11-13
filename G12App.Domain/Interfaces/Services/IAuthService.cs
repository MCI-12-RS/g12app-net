using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<ResponseLoginDTO>> Login(LoginDTO login);
    }
}
