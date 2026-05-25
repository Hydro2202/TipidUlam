using AutoMapper;
using CapstoneAPI.DTOs;
using CapstoneAPI.Models;

namespace CapstoneAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Project mappings
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner != null ? src.Owner.Name : null));
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();
            CreateMap<Project, ProjectDetailsDto>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner != null ? src.Owner.Name : null))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));

            // Task mappings
            CreateMap<Task, TaskDto>()
                .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => src.AssignedTo != null ? src.AssignedTo.Name : null));
            CreateMap<CreateTaskDto, Task>();
            CreateMap<UpdateTaskDto, Task>();
        }
    }
}
