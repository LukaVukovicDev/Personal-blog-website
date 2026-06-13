using AutoMapper;
using Blog.Application.Auth.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Auth.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()));
    }
}
