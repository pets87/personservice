using AutoMapper;
using PersonService.Dtos.Person;
using PersonService.Models;

namespace PersonService.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PersonDto, Person>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.TimeOfDeath, opt => opt.MapFrom(src => src.TimeOfDeath))
                .ForMember(dest => dest.PersonalCode, opt => opt.MapFrom(src => src.PersonalCode));

            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.TimeOfDeath, opt => opt.MapFrom(src => src.TimeOfDeath))
                .ForMember(dest => dest.PersonalCode, opt => opt.MapFrom(src => src.PersonalCode));
        }       
    }
}
