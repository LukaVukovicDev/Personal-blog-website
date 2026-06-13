using AutoMapper;
using Blog.Application.Tags.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Tags.Mapping;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagDto>();
    }
}
