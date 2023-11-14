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
            var totalMembers = memberEntity.Any() ? await _memberRepository.CountMembersAsync(query) : 0;
            return new ServiceResult<IEnumerable<MemberDto>>(
                _mapper.Map<IEnumerable<MemberDto>>(memberEntity),
                totalMembers);
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
            await UpdateComputedValues(memberEntity);

            var member = await _memberRepository.Create(memberEntity);
            return new ServiceResult<MemberDto>(_mapper.Map<MemberDto>(member));
        }

        public async Task<ServiceResult<MemberDto>> Update(MemberDto memberDTO, int id)
        {
            var memberEntity = _mapper.Map<Member>(memberDTO);
            memberEntity.SetupLastModified();
            await UpdateComputedValues(memberEntity);

            var member = await _memberRepository.Update(memberEntity);
            return new ServiceResult<MemberDto>(_mapper.Map<MemberDto>(member));
        }

        public async Task<ServiceResult<MemberDto>> ConsolidateMemberComputedData(int id)
        {
            var memberEntity = await _memberRepository.GetMemberByIdAsync(id);
            await UpdateComputedValues(memberEntity);

            var member = await _memberRepository.Update(memberEntity);
            return new ServiceResult<MemberDto>(_mapper.Map<MemberDto>(member));
        }

        public async Task<ServiceResult> SetConsolidatedChildrenData(int? id, bool flagValue)
        {
            var memberEntity = await _memberRepository.GetMemberByIdAsync(id);
            if (memberEntity == null)
                return new ServiceResult(HttpStatusCode.NotFound, "Não encontrado.");

            memberEntity.ChildrenMembersComputed = true;
            _ = await _memberRepository.Update(memberEntity);

            return new ServiceResult();
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

        private async Task UpdateComputedValues(Member memberEntity)
        {
            var leadershipList = new List<Member>();
            var leadId = memberEntity.LeadershipId;

            while (leadId is > 0)
            {
                var leader = await _memberRepository.GetMemberByIdAsync(leadId);
                leadershipList.Add(leader);
                leadId = leader.LeadershipId;

                if (leadershipList.Count > 5)
                    break;
            }

            if (leadershipList.Select(x => x.Id).Distinct().Count() != leadershipList.Count)
                throw new Exception("Membro não pode ser discípulo de um discípulo.");

            leadershipList.Reverse();

            memberEntity.ComputedLeadershipName = leadershipList.LastOrDefault()?.Name ?? string.Empty;
            memberEntity.ComputedG12Leader = leadershipList.Skip(1).FirstOrDefault()?.Name;
            memberEntity.LeadershipTree = string.Join(" / ", leadershipList.Select(x => x.Name));
            memberEntity.MinistryLevel = (int)Math.Pow(12, leadershipList.Count);
            memberEntity.ChildrenMembersComputed = false;
        }
    }
}
