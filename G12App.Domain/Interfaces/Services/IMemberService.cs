using G12App.Domain.DTOs;
using G12App.Domain.Messages;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace G12App.Domain.Interfaces.Services
{
    public interface IMemberService
    {
        Task<ServiceResult<IEnumerable<MemberDto>>> GetMembers(Query<MemberFilter> query, bool preservePassword = false);
        Task<ServiceResult<MemberDto>> GetMemberById(int? id, bool preservePassword = false);
        Task<ServiceResult<IEnumerable<MemberDtoSmall>>> GetMembersByParentId(int parentId, int page, int limit);
        Task<ServiceResult<MemberDto>> Add(MemberDto memberDTO);
        Task<ServiceResult<MemberDto>> Update(MemberDto memberDTO, int id);
        Task<ServiceResult> Remove(int? id);
        Task<ServiceResult<MemberDto>> ConsolidateMemberComputedData(int id);
        Task<ServiceResult> SetConsolidatedChildrenData(int? id, bool flagValue);
    }
}
