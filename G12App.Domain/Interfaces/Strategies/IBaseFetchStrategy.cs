using G12App.Domain.Model.Base;
using System.Linq;

namespace G12App.Domain.Interfaces.Strategies
{
    public interface IBaseFetchStrategy
    { }

    public interface IBaseFetchStrategy<TEntity> : IBaseFetchStrategy
        where TEntity : BaseEntity
    {
        IQueryable<TEntity> Setup(IQueryable<TEntity> query);
    }
}
