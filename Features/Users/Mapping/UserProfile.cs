using AutoMapper;
using ReactorTwinAPI.Domain.Entities;
using ReactorTwinAPI.Features.Users.Dtos;

namespace ReactorTwinAPI.Features.Users.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
        }
    }
}
