using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Querying;

namespace G12App.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMemberService _memberService;

        public AuthService(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public async Task<ServiceResult<ResponseLoginDTO>> Login(LoginDTO login)
        {
            if (login != null)
            {
                
            }
        }
    }
}
