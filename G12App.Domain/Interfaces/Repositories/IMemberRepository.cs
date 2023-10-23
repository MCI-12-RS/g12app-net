using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetMembersAsync(Query<MemberFilter> query);
        Task<int> CountMembersAsync(Query<MemberFilter> query);
        Task<Member> GetMemberByIdAsync(int? id);
        Task<Member> Create(Member member);
        Task<Member> Update(Member member);
        Task<Member> Remove(Member member);
    }
}
