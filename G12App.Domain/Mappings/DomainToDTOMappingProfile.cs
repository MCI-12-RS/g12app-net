using AutoMapper;
using G12App.Domain.DTOs;
using G12App.Domain.Model;

namespace G12App.Domain.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Member, MemberDTO>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
