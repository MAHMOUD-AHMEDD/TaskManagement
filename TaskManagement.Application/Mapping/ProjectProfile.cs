using AutoMapper;

namespace TaskManagement.Application.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Domain.Entities.Project, DTOs.Project.ProjectDto>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.FullName));
            CreateMap<Domain.Entities.Project, DTOs.Project.CreateProjectDto>();
            CreateMap<Domain.Entities.Project, DTOs.Project.UpdateProjectDto>();
        }
    }
}
