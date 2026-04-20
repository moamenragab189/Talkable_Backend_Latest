using AutoMapper;
using Talkable.Data.DTOs;
using Talkable.Data.Entities;

namespace Talkable.Mapping
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(
                dto=>dto.Full_Name,
                opt=>opt.MapFrom(u=>u.First_Name + " " + u.Last_Name)
                ).ReverseMap();


        }
    }
}
