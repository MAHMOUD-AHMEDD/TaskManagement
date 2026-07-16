using AutoMapper;
using TaskManagement.Application.DTOs.Task;
namespace TaskManagement.Application.Mapping
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Domain.Entities.Task, TaskDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.AssignedUserNames, opt => opt.MapFrom(src => src.TaskAssignments.Select(t => t.User.FullName)))
                .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.TaskLabels.Select(l => l.Label.Name)));
            CreateMap<CreateTaskDto, Domain.Entities.Task>();
            CreateMap<UpdateTaskDto, Domain.Entities.Task>();
        }
    }
}
