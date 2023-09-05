using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using G12App.Domain.Querying;

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

        public async Task<IEnumerable<MemberDto>> GetMembers(Query query)
        {
            var memberEntity = await _memberRepository.GetMembersAsync(query);
            return _mapper.Map<IEnumerable<MemberDto>>(memberEntity);
        }
        public async Task<MemberDto> GetMemberById(int? id)
        {
            var memberEntity = await _memberRepository.GetMemberByIdAsync(id);
            return _mapper.Map<MemberDto>(memberEntity);
        }

        public async Task<MemberDto> Add(MemberDto memberDTO)
        {
            var memberEntity = _mapper.Map<Member>(memberDTO);
            var member = await _memberRepository.Create(memberEntity);
            return _mapper.Map<MemberDto>(member);
        }

        public async Task Update(MemberDto memberDTO)
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
