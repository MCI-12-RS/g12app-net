using G12App.Domain.Model;
using System.Linq;
using System.Security.Claims;

namespace G12App.Domain.Extensions
{
    public static class UserExtensions
    {
        public static bool HasElevatedPermissions(this ClaimsPrincipal user)
        {
            var roles = user.FindAll(ClaimTypes.Role);
            return roles.Any(x => Roles.AppAdmin.Equals(x.Value) || Roles.AppSuperAdmin.Equals(x.Value));
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            var roles = user.FindAll(ClaimTypes.Role);
            return roles.Any(x => Roles.AppAdmin.Equals(x.Value));
        }

        public static bool IsSuperAdmin(this ClaimsPrincipal user)
        {
            var roles = user.FindAll(ClaimTypes.Role);
            return roles.Any(x => Roles.AppSuperAdmin.Equals(x.Value));
        }

        public static int GetMemberId(this ClaimsPrincipal user)
        {
            var memberIdValue = user.FindFirstValue(CustomClaims.MemberId);
            int.TryParse(memberIdValue, out var memberId);
            return memberId;
        }
    }
}
