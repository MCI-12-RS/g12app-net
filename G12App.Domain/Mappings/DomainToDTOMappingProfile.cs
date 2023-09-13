using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Model;

namespace G12App.Domain.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Member, MemberDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Cell, CellDTO>().ReverseMap();

            CreateMap<Member, Member>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Address, Address>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Cell, Cell>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
