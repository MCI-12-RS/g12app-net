using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace G12App.Infrastructure.Data.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppContext _context;

        public MemberRepository(AppContext context)
        {
            _context = context;
        }

        public IQueryable<Member> Get()
        {
            return _context.Members.AsNoTracking();
        }
    }
}
