using G12App.Domain.Model.Base;

namespace G12App.Domain.Interfaces.Services
{
    public interface IEntityChangeTrackingService
    {
        void SetupCreationData(BaseEntity entity);
        void SetupLastModifiedData(BaseEntity entity);
    }
}
