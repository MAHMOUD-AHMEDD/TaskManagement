using AutoMapper;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // Entity -> DTO mapping with a safety check if Owner is not yet loaded
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner != null ? src.Owner.FullName : string.Empty));

            // DTO -> Entity mappings (Fixed direction)
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();
        }
    }
}
