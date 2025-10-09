using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Repositories;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Messages;
using G12App.Domain.Model;
using G12App.Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace G12App.Application.Services
{
    public class MinistryService : IMinistryService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public MinistryService(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<IEnumerable<CoupleDto>>> GetCouplesByParentIdAsync(int parentId, int page, int limit)
        {
            if (parentId < 0)
            {
                return new ServiceResult<IEnumerable<CoupleDto>>(Array.Empty<CoupleDto>(), HttpStatusCode.BadRequest, "Identificador de liderança inválido.");
            }

            var members = await _memberRepository.GetChildrenWithSpouseAsync(parentId);

            var couples = BuildCouples(members)
                .OrderBy(c => c.Husband?.Name ?? c.Wife?.Name ?? string.Empty)
                .ToList();

            var currentPage = Math.Max(page, 1);
            var pageSize = limit <= 0 ? 12 : Math.Min(limit, 120);
            var skip = (currentPage - 1) * pageSize;

            var paginatedCouples = couples.Skip(skip).Take(pageSize).ToList();

            return new ServiceResult<IEnumerable<CoupleDto>>(paginatedCouples, couples.Count);
        }

        private IReadOnlyCollection<CoupleDto> BuildCouples(IEnumerable<Member> members)
        {
            var couples = new List<CoupleDto>();
            var processedIds = new HashSet<int>();

            foreach (var member in members)
            {
                if (processedIds.Contains(member.Id))
                    continue;

                var spouse = member.Spouse;

                Member husband = null;
                Member wife = null;

                if (spouse != null)
                {
                    processedIds.Add(spouse.Id);
                    (husband, wife) = SortByGender(member, spouse);
                }
                else
                {
                    if (member.Gender == Gender.Male)
                        husband = member;
                    else if (member.Gender == Gender.Female)
                        wife = member;
                    else
                        husband = member;
                }

                processedIds.Add(member.Id);

                couples.Add(new CoupleDto
                {
                    Husband = husband != null ? _mapper.Map<MemberDtoSmall>(husband) : null,
                    Wife = wife != null ? _mapper.Map<MemberDtoSmall>(wife) : null
                });
            }

            return couples;
        }

        private static (Member husband, Member wife) SortByGender(Member first, Member second)
        {
            Member husband = null;
            Member wife = null;

            if (first.Gender == Gender.Male)
                husband = first;
            else if (first.Gender == Gender.Female)
                wife = first;

            if (second.Gender == Gender.Male)
                husband = husband ?? second;
            else if (second.Gender == Gender.Female)
                wife = wife ?? second;

            husband ??= first;
            wife ??= second;

            return (husband, wife);
        }
    }
}
