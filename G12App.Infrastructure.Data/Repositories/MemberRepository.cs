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
            var trackedMember = _context.Members.Local.FirstOrDefault(x => x.Id == member.Id);
            if (trackedMember != null && !ReferenceEquals(trackedMember, member))
            {
                _context.Entry(trackedMember).State = EntityState.Detached;
            }

            var memberEntry = _context.Entry(member);
            if (memberEntry.State == EntityState.Detached)
            {
                _context.Attach(member);
                memberEntry = _context.Entry(member);
            }

            memberEntry.State = EntityState.Modified;

            if (member.HomeAddress != null && member.HomeAddress.Id > 0)
            {
                var trackedAddress = _context.Addresses.Local.FirstOrDefault(x => x.Id == member.HomeAddress.Id);
                if (trackedAddress != null && !ReferenceEquals(trackedAddress, member.HomeAddress))
                {
                    _context.Entry(trackedAddress).State = EntityState.Detached;
                }

                var addressEntry = _context.Entry(member.HomeAddress);
                if (addressEntry.State == EntityState.Detached)
                {
                    _context.Attach(member.HomeAddress);
                    addressEntry = _context.Entry(member.HomeAddress);
                }

                addressEntry.State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return member;
        }
    }
}
