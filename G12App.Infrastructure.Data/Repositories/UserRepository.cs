using AutoMapper;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using G12App.Infrastructure.Data.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<User> Create(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<User> GetUserByIdAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync(Query<UserFilter> query)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> Remove(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> Update(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
