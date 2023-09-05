using G12App.Domain.DTOs;
using G12App.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDTO>> GetMembers();
        Task<MemberDTO> GetMemberById(int? id);
        Task Add(MemberDTO memberDTO);
        Task Update(MemberDTO memberDTO);
        Task Remove(int? id);
    }
}
