using G12App.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G12App.Application.Interfaces
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
