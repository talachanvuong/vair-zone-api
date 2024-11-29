using AutoMapper;
using VAirZoneWebAPI.DTOs.User;
using VAirZoneWebAPI.Models.User;

namespace VAirZoneWebAPI.Mappers
{
	public class UserProfile : Profile
	{
        public UserProfile()
        {
            CreateMap<UserModel, UserDto>();
        }
    }
}