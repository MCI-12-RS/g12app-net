using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Extensions;
using G12App.Domain.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace G12App.Domain.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile(IHttpContextAccessor httpContextAccessor)
        {
            CreateMap<Member, MemberDto>()
                .ForMember(x => x.Password, opt =>
                {
                    opt.PreCondition(c =>
                    {
                        if (c.TryGetItems(out var items))
                            if (items.TryGetValue(MappingConstants.PreservePassword, out var preservePassword))
                                return Convert.ToBoolean(preservePassword);
                        return false;
                    });
                    opt.MapFrom(s => s.Password);
                });
            CreateMap<MemberDto, Member>()
                .ForMember(x => x.Leadership, opt => opt.Ignore())
                .ForMember(x => x.Password, opt =>
                {
                    opt.PreCondition(x => !string.IsNullOrWhiteSpace(x.Password) &&
                                        (httpContextAccessor.HttpContext.User.GetMemberId() == x.Id ||
                                            httpContextAccessor.HttpContext.User.HasElevatedPermissions()));
                    opt.MapFrom(s => s.Password);
                })
                .ForMember(x => x.Role, opt =>
                {
                    opt.PreCondition(x => !string.IsNullOrWhiteSpace(x.Role) &&
                                            httpContextAccessor.HttpContext.User.IsSuperAdmin());
                    opt.MapFrom(s => s.Role);
                });

            CreateMap<Cell, CellDto>()
                .ForMember(x => x.Time, opt => opt.MapFrom(x => TimeOnly.Parse(x.Time)));

            CreateMap<CellDto, Cell>()
                .ForMember(x => x.Lead, opt => opt.Ignore())
                .ForMember(x => x.Host, opt => opt.Ignore())
                .ForMember(x => x.Timothy, opt => opt.Ignore())
                .ForMember(x => x.Time, opt => opt.MapFrom(x => x.Time.ToString()));

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<Report, ReportDto>()
                .ForMember(x => x.Visitors, opt => opt.MapFrom(m => m.Visitors.Split("|", StringSplitOptions.RemoveEmptyEntries)));

            CreateMap<ReportDto, Report>()
                .ForMember(x => x.Visitors, opt => opt.MapFrom(m => string.Join("|", m.Visitors.Where(x => !string.IsNullOrWhiteSpace(x)))));

            CreateMap<Member, Member>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Address, Address>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Cell, Cell>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Report, Report>()
                .ForMember(x => x.Id, opt => opt.Ignore());

        }
    }
}
