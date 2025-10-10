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

        public async Task<List<MemberSmall>> GetChildrenWithSpouseAsync(IEnumerable<int> parentIds)
        {
            var filters = parentIds?.Where(id => id > 0).Distinct().ToList() ?? new List<int>();

            IQueryable<Member> query = _context.Members
                .Where(x => x.IsDeleted == false)
                .ApplyMinistryFilter(_httpContextAccessor?.HttpContext?.User);

            if (filters.Count == 0)
            {
                var lowestMinistryLevel = query
                    .OrderBy(x => x.MinistryLevel)
                    .Select(x => x.MinistryLevel)
                    .FirstOrDefault();

                if (lowestMinistryLevel == 0)
                {
                    query = query.Where(x => x.MinistryLevel == 0);
                }
                else if (lowestMinistryLevel.HasValue)
                {
                    query = query.Where(x => x.MinistryLevel == lowestMinistryLevel.Value);
                }
            }
            else
                query = query.Where(x => x.LeadershipId.HasValue && filters.Contains(x.LeadershipId.Value));

            var members = await query
                .OrderBy(x => x.Name)
                .Select(x => new MemberSmall
                {
                    Id = x.Id,
                    Name = x.Name,
                    Gender = x.Gender,
                    LeadershipId = x.LeadershipId,
                    MinistryLevel = x.MinistryLevel,
                    SpouseId = x.SpouseId,
                    ComputedG12Leader = x.ComputedG12Leader,
                    ComputedLeadershipName = x.ComputedLeadershipName,
                    TotalDisciples = x.TotalDisciples,
                    CellsLeadingCount = x.CellsLeading.Count(),
                    CellsHostingCount = x.CellsHosting.Count(),
                    CellsServingCount = x.CellsServing.Count(),
                    TotalCells = x.TotalCells,
                    Baptized = x.Baptized,
                    Spouse = x.Spouse == null ? null : new MemberSmall
                    {
                        Id = x.Spouse.Id,
                        Name = x.Spouse.Name,
                        Gender = x.Spouse.Gender,
                        LeadershipId = x.Spouse.LeadershipId,
                        MinistryLevel = x.Spouse.MinistryLevel,
                        SpouseId = x.Spouse.SpouseId,
                        ComputedG12Leader = x.Spouse.ComputedG12Leader,
                        ComputedLeadershipName = x.Spouse.ComputedLeadershipName,
                        TotalDisciples = x.Spouse.TotalDisciples,
                        CellsLeadingCount = x.Spouse.CellsLeading.Count(),
                        CellsHostingCount = x.Spouse.CellsHosting.Count(),
                        CellsServingCount = x.Spouse.CellsServing.Count(),
                        TotalCells = x.Spouse.TotalCells,
                        Baptized = x.Baptized,
                    }
                })
                .ToListAsync();
            return members;
        }
    }
}
