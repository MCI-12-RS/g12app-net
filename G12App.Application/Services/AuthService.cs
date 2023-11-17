using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace G12App.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMemberService _memberService;
        private readonly JWTSettings _jwtSettings;

        public AuthService(IMemberService memberService, IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _memberService = memberService;
        }
        public async Task<ServiceResult<ResponseLoginDTO>> Login(LoginDTO login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.UserName) || string.IsNullOrWhiteSpace(login.Password))
                return new ServiceResult<ResponseLoginDTO>(null, HttpStatusCode.BadRequest, "Preencha o usuario e senha.");

            var query = new Query<MemberFilter>()
            {
                Pagination = new Pagination(1, 1),
                Filter = new MemberFilter()
                {
                    UserName = login.UserName,
                }
            };
            var result = await _memberService.GetMembers(query);
            var user = result.Data.FirstOrDefault();

            if (user == null)
                return new ServiceResult<ResponseLoginDTO>(null, HttpStatusCode.BadRequest, "Usuário não encontrado.");

            if (login.Password != user.Password)
                return new ServiceResult<ResponseLoginDTO>(null, HttpStatusCode.BadRequest, "Senha incorreta.");

            var response = new ResponseLoginDTO
            {
                UserName = login.UserName,
                Token = GenerateAccessToken(user),
                MemberId = user.Id.GetValueOrDefault(),
                Name = user.Name,

            };
            return new ServiceResult<ResponseLoginDTO>(response);
        }

        private string GenerateAccessToken(MemberDto member)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, member.Name),
                    new("MemberId", member.Id.ToString()),
                    new(ClaimTypes.Email, member.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
