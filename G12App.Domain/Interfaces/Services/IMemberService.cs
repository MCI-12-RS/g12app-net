using G12App.Domain.DTOs;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetMembers(Query query);
        Task<MemberDto> GetMemberById(int? id);
        Task<MemberDto> Add(MemberDto memberDTO);
        Task Update(MemberDto memberDTO);
        Task Remove(int? id);
    }
}
