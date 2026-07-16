using AutoMapper;
using TaskManagement.Application.DTOs.Label;
using TaskManagement.Domain.Entities;
namespace TaskManagement.Application.Mapping
{
    public class LabelProfile : Profile
    {
        public LabelProfile()
        {
            CreateMap<Label, LabelDto>();
            CreateMap<CreateLabelDto, Label>();
            CreateMap<UpdateLabelDto, Label>();
        }
    }
}
