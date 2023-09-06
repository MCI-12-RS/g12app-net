using AutoMapper;
using G12App.Common.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Querying;
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
        private readonly IMapper _mapper;

        public MemberRepository(
            AppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Member> Create(Member member)
        {
            _context.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member> GetMemberByIdAsync(int? id)
        {
            return await _context.Members.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<IEnumerable<Member>> GetMembersAsync(Query query)
        {
            return await _context.Members
                .Where(x => x.IsDeleted == false)
                .ApplyPaging(query)
                .ToListAsync();
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
            var memberEntity = await _context.Members.FirstOrDefaultAsync(x => x.Id == member.Id && x.IsDeleted == false);
            _mapper.Map(member, memberEntity);
            _context.Entry(memberEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return member;
        }
    }
}
