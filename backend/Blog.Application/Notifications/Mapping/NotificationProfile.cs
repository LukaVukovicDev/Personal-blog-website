using AutoMapper;
using Blog.Application.Notifications.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Notifications.Mapping;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()));
    }
}
