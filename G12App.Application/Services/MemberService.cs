using AutoMapper;
using G12App.Application.Events;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Events;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Mappings;
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
        private readonly IEntityChangeTrackingService _changeTrackingService;
        private readonly IEventDispatcher _eventDispatcher;

        public MemberService(
            IMemberRepository memberRepository,
            IMapper mapper,
            IEntityChangeTrackingService changeTrackingService,
            IEventDispatcher eventDispatcher)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
            _changeTrackingService = changeTrackingService;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<ServiceResult<IEnumerable<MemberDto>>> GetMembers(Query<MemberFilter> query, bool preservePassword = false)
        {
            var memberEntity = await _memberRepository.GetMembersAsync(query);
            var totalMembers = memberEntity.Any() ? await _memberRepository.CountMembersAsync(query) : 0;
            return new ServiceResult<IEnumerable<MemberDto>>(
                _mapper.Map<IEnumerable<MemberDto>>(memberEntity, opt => opt.Items[MappingConstants.PreservePassword] = preservePassword),
                totalMembers);
        }

        public async Task<ServiceResult<MemberDto>> GetMemberById(int? id, bool preservePassword = false)
        {
            var memberEntity = await _memberRepository.GetMemberByIdAsync(id);
            return new ServiceResult<MemberDto>(_mapper.Map<MemberDto>(memberEntity, opt => opt.Items[MappingConstants.PreservePassword] = preservePassword));
        }

        public async Task<ServiceResult<MemberDto>> Add(MemberDto memberDTO)
        {
            var memberEntity = _mapper.Map<Member>(memberDTO);

            _changeTrackingService.SetupCreationData(memberEntity);
            _changeTrackingService.SetupLastModifiedData(memberEntity);

            await UpdateComputedValues(memberEntity);

            var member = await _memberRepository.Create(memberEntity);
            await _eventDispatcher.DispatchEvent(new MemberUpserted(member.Id));

            return new ServiceResult<MemberDto>(_mapper.Map<MemberDto>(member));
        }

        public async Task<ServiceResult<MemberDto>> Update(MemberDto memberDTO, int id)
        {
            var memberEntity = await _memberRepository.GetMemberByIdAsync(id);
            memberEntity = _mapper.Map(memberDTO, memberEntity);

            _changeTrackingService.SetupLastModifiedData(memberEntity);
            await UpdateComputedValues(memberEntity);

            var member = await _memberRepository.Update(memberEntity);
            await _eventDispatcher.DispatchEvent(new MemberUpserted(member.Id));

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

            _changeTrackingService.SetupLastModifiedData(memberEntity);
            await _memberRepository.Remove(memberEntity);
            return new ServiceResult();
        }

        private async Task UpdateComputedValues(Member memberEntity)
        {
            var leadershipList = new List<Member>();
            var leadershipIdList = new List<int?>();

            var leadId = memberEntity.LeadershipId;
            Member leader = memberEntity.Leadership;

            while (leadId is > 0)
            {
                leader = leader ?? await _memberRepository.GetMemberByIdAsync(leadId);
                leadershipList.Add(leader);
                leadershipIdList.Add(leadId);

                if (leadershipList.Count > 5)
                    break;

                leadId = leader.LeadershipId;
                leader = leader.Leadership;
            }

            if (leadershipList.Select(x => x.Id).Distinct().Count() != leadershipList.Count)
                throw new Exception("Membro não pode ser discípulo de um discípulo.");

            leadershipList.Reverse();
            leadershipIdList.Reverse();

            memberEntity.ComputedLeadershipName = leadershipList.LastOrDefault()?.Name ?? string.Empty;
            memberEntity.ComputedG12Leader = leadershipList.Skip(1).FirstOrDefault()?.Name;
            memberEntity.LeadershipTree = string.Join(" / ", leadershipList.Select(x => x.Name));
            memberEntity.LeadershipIdTree = $"|{string.Join("|", leadershipIdList.Where(x => x.HasValue))}|";
            memberEntity.MinistryLevel = (int)Math.Pow(12, leadershipList.Count);
            memberEntity.ChildrenMembersComputed = false;
        }
    }
}
