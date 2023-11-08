using AutoMapper;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using G12App.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<User> GetUserByIdAsync(int? id)
        {
            return await _context.Users.FirstOrDefaultAsync
                 (x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(Query<UserFilter> query)
        {
            return await _context.Users
                            .Where(x => x.IsDeleted == false)
                            .ApplyPaging(query)
                            .ApplyFilter(query)
                            .ToListAsync();
        }

        public async Task<User> Remove(User user)
        {
            if (_context.Entry(user).State == EntityState.Detached)
                _context.Attach(user);
            user.IsDeleted = true;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(User user)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id && x.IsDeleted == false);
            _mapper.Map(user, userEntity);
            _context.Entry(userEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
