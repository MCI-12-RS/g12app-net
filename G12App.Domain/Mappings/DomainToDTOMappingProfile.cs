using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Model;

namespace G12App.Domain.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Member, MemberDto>();
            CreateMap<MemberDto, Member>()
                .ForMember(x => x.Leadership, opt => opt.Ignore());

            CreateMap<Cell, CellDTO>();
            CreateMap<CellDTO, Cell>()
                .ForMember(x => x.Lead, opt => opt.Ignore())
                .ForMember(x => x.Host, opt => opt.Ignore())
                .ForMember(x => x.Timothy, opt => opt.Ignore());

            CreateMap<Address, AddressDto>().ReverseMap();
            
            CreateMap<Report, ReportDTO>().ReverseMap();

            CreateMap<Member, Member>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Address, Address>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Cell, Cell>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Report, Report>()
                .ForMember(x => x.ReportId, opt => opt.Ignore());
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
