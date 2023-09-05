using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Infrastructure.Data.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppContext _context;

        public MemberRepository(AppContext context)
        {
            _context = context;
        }

        public async Task<Member> Create(Member member)
        {
            _context.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member> GetMemberByIdAsync(int? id)
        {
            return await _context.Members.FindAsync(id);
        }

        public async Task<IEnumerable<Member>> GetMembersAsync()
        {
            return await _context.Members
                .Include(x => x.HomeAddress)
                .ToListAsync();
        }

        public async Task<Member> Remove(Member member)
        {
            _context.Remove(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member> Update(Member member)
        {
            _context.Update(member);
            await _context.SaveChangesAsync();
            return member;
        }
    }
}
