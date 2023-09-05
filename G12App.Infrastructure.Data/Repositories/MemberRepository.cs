using AutoMapper;
using G12App.Common.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
            return await _context.Members.FindAsync(id);
        }

        public async Task<IEnumerable<Member>> GetMembersAsync(Query query)
        {
            return await _context.Members
                .Include(x => x.HomeAddress)
                .ApplyPaging(query)
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
            var memberEntity = await _context.Members.Include(x => x.HomeAddress).FirstOrDefaultAsync(x => x.Id == member.Id);
            _mapper.Map(member, memberEntity);
            _context.Entry(memberEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return member;
        }
    }
}
