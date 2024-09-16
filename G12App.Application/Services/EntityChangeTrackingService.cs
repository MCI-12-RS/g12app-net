using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using G12App.Domain.Model.Base;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace G12App.Application.Services
{
    public class EntityChangeTrackingService : IEntityChangeTrackingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EntityChangeTrackingService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetupCreationData(BaseEntity entity)
        {
            var userName = _httpContextAccessor.HttpContext?
                .User?.Claims.FirstOrDefault(x => ClaimTypes.Name.Equals(x.Type))?
                .Value;

            entity.CreatedBy = userName;
            entity.CreationDate = SystemConstants.BrazilTime.DateTime;
        }

        public void SetupLastModifiedData(BaseEntity entity)
        {
            var userName = _httpContextAccessor.HttpContext?
                .User?.Claims.FirstOrDefault(x => ClaimTypes.Name.Equals(x.Type))?
                .Value;

            entity.LastModifiedBy = userName;
            entity.LastModifiedDate = SystemConstants.BrazilTime.DateTime;
        }
    }
}
