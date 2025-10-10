using AutoMapper;
using G12App.Application.Events;
using G12App.Domain.DomainEvents;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Events;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Mappings;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System;
using System.Collections.Generic;
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
            var memberDto = _mapper.Map<MemberDto>(memberEntity, opt => opt.Items[MappingConstants.PreservePassword] = preservePassword);
            return new ServiceResult<MemberDto>(memberDto);
        }

        public async Task<ServiceResult<IEnumerable<MemberDtoSmall>>> GetMembersByParentId(int parentId, int page, int limit)
        {
            if (parentId < 0)
            {
                return new ServiceResult<IEnumerable<MemberDtoSmall>>(Array.Empty<MemberDtoSmall>(), HttpStatusCode.BadRequest, "Identificador de liderança inválido.");
            }

            var currentPage = Math.Max(page, 1);
            var pageSize = limit <= 0 ? 12 : Math.Min(limit, 120);

            var query = new Query<MemberFilter>
            {
                Pagination = new Pagination(currentPage, pageSize),
                Sort = new Sort(nameof(Member.Name), order: false)
            };

            if (parentId == 0)
                query.Filter.MinistryLevel = 1;
            else
                query.Filter.LeadershipId = parentId;

            var members = await _memberRepository.GetMembersAsync(query);
            var totalMembers = await _memberRepository.CountMembersAsync(query);

            var mappedMembers = _mapper.Map<IEnumerable<MemberDtoSmall>>(members);

            return new ServiceResult<IEnumerable<MemberDtoSmall>>(mappedMembers, totalMembers);
        }

        public async Task<ServiceResult<MemberDto>> Add(MemberDto memberDTO)
        {
            var memberEntity = _mapper.Map<Member>(memberDTO);

            _changeTrackingService.SetupCreationData(memberEntity);
            _changeTrackingService.SetupLastModifiedData(memberEntity);

            await UpdateComputedValues(memberEntity);

            var member = await _memberRepository.Create(memberEntity);
            var resultMemberDto = _mapper.Map<MemberDto>(member);

            await UpdateSpouse(member, null, member.SpouseId);

            await _eventDispatcher.DispatchEvent(new MemberCreated(resultMemberDto));

            return new ServiceResult<MemberDto>(resultMemberDto);
        }

        public async Task<ServiceResult<MemberDto>> Update(MemberDto memberDTO, int id)
        {
            var memberEntity = await _memberRepository.GetMemberByIdAsync(id);
            var beforeSpouseId = memberEntity?.SpouseId;
            var sourceMemberDto = _mapper.Map<Member, MemberDto>(memberEntity);

            memberEntity = _mapper.Map(memberDTO, memberEntity);
            var afterSpouseId = memberEntity.SpouseId;

            _changeTrackingService.SetupLastModifiedData(memberEntity);
            await UpdateComputedValues(memberEntity);

            var member = await _memberRepository.Update(memberEntity);
            var resultMemberDto = _mapper.Map<MemberDto>(member);

            await UpdateSpouse(member, beforeSpouseId, afterSpouseId);

            await _eventDispatcher.DispatchEvent(new MemberUpdated(sourceMemberDto, resultMemberDto));

            return new ServiceResult<MemberDto>(resultMemberDto);
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

            var sourceMemberDto = _mapper.Map<Member, MemberDto>(memberEntity);

            _changeTrackingService.SetupLastModifiedData(memberEntity);
            await _memberRepository.Remove(memberEntity);

            await UpdateSpouse(memberEntity, memberEntity.SpouseId, null);

            await _eventDispatcher.DispatchEvent(new MemberDeleted(sourceMemberDto));

            return new ServiceResult();
        }

        private async Task UpdateSpouse(Member member, int? beforeSpouseId, int? afterSpouseId)
        {
            if (beforeSpouseId.GetValueOrDefault() == afterSpouseId.GetValueOrDefault())
                return;

            if (beforeSpouseId.GetValueOrDefault() != 0)
            {
                var beforeSpouse = await _memberRepository.GetMemberByIdAsync(beforeSpouseId);
                beforeSpouse.SpouseId = null;
                await _memberRepository.Update(beforeSpouse);
            }

            if (afterSpouseId.GetValueOrDefault() != 0)
            {
                var afterSpouse = await _memberRepository.GetMemberByIdAsync(afterSpouseId);
                afterSpouse.SpouseId = member.Id;
                await _memberRepository.Update(afterSpouse);
            }
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

            if (memberEntity.HasNoLeadership)
            {
                memberEntity.MinistryLevel = 9999999;
                memberEntity.ComputedLeadershipName = null;
                memberEntity.ComputedG12Leader = null;
                memberEntity.ComputedG12LeaderId = null;
                memberEntity.ComputedG12LeaderSpouseId = null;
                memberEntity.LeadershipTree = null;
                memberEntity.LeadershipIdTree = null;
                memberEntity.ChildrenMembersComputed = true;
                return;
            }

            memberEntity.ComputedLeadershipName = leadershipList.LastOrDefault()?.Name ?? string.Empty;
            memberEntity.ComputedG12Leader = leadershipList.Skip(1).FirstOrDefault()?.Name;
            memberEntity.ComputedG12LeaderId = leadershipList.Skip(1).FirstOrDefault()?.Id;
            memberEntity.ComputedG12LeaderSpouseId = leadershipList.Skip(1).FirstOrDefault()?.SpouseId;
            memberEntity.LeadershipTree = string.Join(" / ", leadershipList.Select(x => x.Name));
            memberEntity.LeadershipIdTree = $"|{string.Join("|", leadershipIdList.Where(x => x.HasValue))}|";
            memberEntity.MinistryLevel = (int)Math.Pow(12, leadershipList.Count);
            memberEntity.ChildrenMembersComputed = false;
        }
    }
}
