using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(Query<UserFilter> query);
        Task<User> GetUserByIdAsync(int? id);
        Task<User> Create(User user);
        Task<User> Update(User user);
        Task<User> Remove(User user);

    }
}
