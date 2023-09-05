using AutoMapper;
using G12App.Application.DTOs;
using G12App.Domain.Model;

namespace G12App.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Member, MemberDTO>().ReverseMap();
        }
    }
}
