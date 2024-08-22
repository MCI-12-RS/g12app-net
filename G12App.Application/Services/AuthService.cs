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
        private readonly AdminUser _adminUser;

        public AuthService(
            IMemberService memberService,
            IOptions<JWTSettings> jwtSettings,
            IOptions<AdminUser> adminUser
            )
        {
            _memberService = memberService;
            _jwtSettings = jwtSettings.Value;
            _adminUser = adminUser.Value;
        }

        public async Task<ServiceResult<ResponseLoginDto>> Login(LoginDto login)
        {
            MemberDto user = null;
            if (login.UserName == _adminUser.UserName && login.Password == _adminUser.Password)
            {
                user = new MemberDto()
                {
                    Id = -1,
                    Name = nameof(AdminUser),
                    Role = Roles.AppSuperAdmin,
                };
            }
            else
            {
                if (login == null || string.IsNullOrWhiteSpace(login.UserName) || string.IsNullOrWhiteSpace(login.Password))
                    return new ServiceResult<ResponseLoginDto>(null, HttpStatusCode.BadRequest, "Preencha o usuario e senha.");

                var query = new Query<MemberFilter>()
                {
                    Pagination = new Pagination(1, 1),
                    Filter = new MemberFilter()
                    {
                        UserName = login.UserName,
                    }
                };

                var result = await _memberService.GetMembers(query, true);
                user = result.Data.FirstOrDefault();

                if (user == null)
                    return new ServiceResult<ResponseLoginDto>(null, HttpStatusCode.BadRequest, "Usuário não encontrado.");

                if (login.Password != user.Password)
                    return new ServiceResult<ResponseLoginDto>(null, HttpStatusCode.BadRequest, "Senha incorreta.");
            }

            var response = new ResponseLoginDto
            {
                UserName = login.UserName,
                Token = GenerateAccessToken(user),
                MemberId = user.Id.GetValueOrDefault(),
                Name = user.Name,
                Role = user.Role,
                G12LeadershipName = user.ComputedG12Leader,
                SpouseId = user.SpouseId,
            };

            return new ServiceResult<ResponseLoginDto>(response);
        }

        private string GenerateAccessToken(MemberDto member)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new(ClaimTypes.Name, member.Name ?? string.Empty),
                    new(CustomClaims.MemberId, member.Id.ToString() ?? string.Empty),
                    new(ClaimTypes.Email, member.Email ?? string.Empty),
                    new(CustomClaims.MinistryLevel, member.MinistryLevel?.ToString() ?? string.Empty),
                    new(CustomClaims.LeadershipId, member.LeadershipId?.ToString() ?? string.Empty),
                    new(ClaimTypes.Role, string.IsNullOrWhiteSpace(member.Role) ? Roles.AppUser : member.Role),
                    new(CustomClaims.G12LeadershipName, member.ComputedG12Leader ?? string.Empty),
                    new(CustomClaims.SpouseId, member.SpouseId?.ToString() ?? string.Empty),
                ]),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
