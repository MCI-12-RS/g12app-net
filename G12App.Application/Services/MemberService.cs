using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;

namespace G12App.Application.Services
{
    public class MemberService : IMemberService
    {
        private IMemberRepository _memberRepository;
        private readonly IMapper _mapper;
        public MemberService(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberDTO>> GetMembers()
        {
            var memberEntity = await _memberRepository.GetMembersAsync();
            return _mapper.Map<IEnumerable<MemberDTO>>(memberEntity);
        }
        public async Task<MemberDTO> GetMemberById(int? id)
        {
            var memberEntity = await _memberRepository.GetMemberByIdAsync(id);
            return _mapper.Map<MemberDTO>(memberEntity);
        }

        public async Task Add(MemberDTO memberDTO)
        {
            var memberEntity = _mapper.Map<Member>(memberDTO);
            await _memberRepository.Create(memberEntity);
        }

        public async Task Update(MemberDTO memberDTO)
        {
            var memberEntity = _mapper.Map<Member>(memberDTO);
            await _memberRepository.Update(memberEntity);
        }

        public async Task Remove(int? id)
        {
            var memberEntity = _memberRepository.GetMemberByIdAsync(id).Result;
            await _memberRepository.Remove(memberEntity);

        }
    }
}
