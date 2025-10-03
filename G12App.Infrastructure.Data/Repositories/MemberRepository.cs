using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using G12App.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Infrastructure.Data.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MemberRepository(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Member> Create(Member member)
        {
            _context.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member> GetMemberByIdAsync(int? id)
        {
            return await _context.Members
                    .Include(x => x.Leadership)
                    .Include(x => x.HomeAddress)
                    .Include(x => x.Spouse)
                    .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Member> GetMembersQueryableAsync(Query<MemberFilter> query)
        {
            return _context.Members
                .IgnoreAutoIncludes()
                .Where(x => x.IsDeleted == false)
                .ApplyMinistryFilter(_httpContextAccessor?.HttpContext?.User)
                .ApplyFilter(query);
        }

        public async Task<IEnumerable<Member>> GetMembersAsync(Query<MemberFilter> query)
        {
            return await
                this.GetMembersQueryableAsync(query)
                    .Include(x => x.HomeAddress)
                    .Include(x => x.Spouse)
                    .ApplySort(query).ThenBy(x => x.Name)
                    .ApplyPaging(query)
                    .ToListAsync();
        }

        public async Task<int> CountMembersAsync(Query<MemberFilter> query)
        {
            return await
                this.GetMembersQueryableAsync(query)
                    .CountAsync();
        }

        public async Task<Member> Remove(Member member)
        {
            if (_context.Entry(member).State == EntityState.Detached)
                _context.Attach(member);
            member.IsDeleted = true;

            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member> Update(Member member)
        {
            _context.Attach(member);
            _context.Entry(member).State = EntityState.Modified;
            if (member.HomeAddress != null && member.HomeAddress.Id > 0)
                _context.Entry(member.HomeAddress).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return member;
        }
    }
}
