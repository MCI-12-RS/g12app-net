using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System.Net;

namespace G12App.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public MemberService(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IEnumerable<MemberDto>>> GetMembers(Query<MemberFilter> query)
        {
            var memberEntity = await _memberRepository.GetMembersAsync(query);
            return new ServiceResult<IEnumerable<MemberDto>>(_mapper.Map<IEnumerable<MemberDto>>(memberEntity));
        }

        public async Task<ServiceResult<MemberDto>> GetMemberById(int? id)
        {
            var memberEntity = await _memberRepository.GetMemberByIdAsync(id);
            return new ServiceResult<MemberDto>(_mapper.Map<MemberDto>(memberEntity));
        }

        public async Task<ServiceResult<MemberDto>> Add(MemberDto memberDTO)
        {
            var memberEntity = _mapper.Map<Member>(memberDTO);
            memberEntity.SetupCreation();
            memberEntity.SetupLastModified();
            var member = await _memberRepository.Create(memberEntity);
            return new ServiceResult<MemberDto>(_mapper.Map<MemberDto>(member));
        }

        public async Task<ServiceResult<MemberDto>> Update(MemberDto memberDTO, int id)
        {
            var memberEntity = _mapper.Map<Member>(memberDTO);
            memberEntity.SetupLastModified();
            var member = await _memberRepository.Update(memberEntity);
            return new ServiceResult<MemberDto>(_mapper.Map<MemberDto>(member));
        }

        public async Task<ServiceResult> Remove(int? id)
        {
            var memberEntity = await _memberRepository.GetMemberByIdAsync(id);
            if (memberEntity == null)
                return new ServiceResult(HttpStatusCode.NotFound, "Não encontrado.");

            memberEntity.SetupLastModified();
            await _memberRepository.Remove(memberEntity);
            return new ServiceResult();
        }
    }
}
