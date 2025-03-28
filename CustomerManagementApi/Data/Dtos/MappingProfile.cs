using AutoMapper;
using CustomerManagementApi.Data.Models;

namespace CustomerManagementApi.Data.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mappatura Customer -> CustomerDto
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.CustomerTags.Select(ct => ct.Tag.Name).ToList()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.HasValue ? src.DateOfBirth.Value.ToString("yyyy-MM-dd") : null))
                .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects));

            // Mappatura CustomerDto -> Customer
            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.CustomerTags, opt => opt.Ignore()) // Ignora i tag, da gestire separatamente
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            // Mappatura Project -> ProjectDto e viceversa
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
        }
    }
}
