using G12App.Domain.Model;
using G12App.Domain.Querying;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetMembersAsync(Query query);
        Task<Member> GetMemberByIdAsync(int? id);
        Task<Member> Create(Member member);
        Task<Member> Update(Member member);
        Task<Member> Remove(Member member);
    }
}
