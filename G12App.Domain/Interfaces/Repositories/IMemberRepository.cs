using G12App.Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetMembersAsync();
        Task<Member> GetMemberByIdAsync(int? id);
        Task<Member> Create(Member member);
        Task<Member> Update(Member member);
        Task<Member> Remove(Member member);
    }
}
