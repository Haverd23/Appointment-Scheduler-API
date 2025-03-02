using AutoMapper;
using Scheduler.DTOs.User;

namespace Scheduler.DTOs.Mappings
{
    public class MappingDTOProfile : Profile
    {
        public MappingDTOProfile() 
        { 
            CreateMap<Scheduler.Models.User, UserLoginDTO>().ReverseMap();
            CreateMap<Scheduler.Models.User, RegisterDTO>().ReverseMap();
            CreateMap<Scheduler.Models.User, UpdateDTO>().ReverseMap();
            CreateMap<Scheduler.Models.User, GetUserDTO>().ReverseMap();

        }
    }
}
