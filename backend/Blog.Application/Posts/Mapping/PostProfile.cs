using AutoMapper;
using Blog.Application.Posts.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Posts.Mapping;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostListItemDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.AuthorName, o => o.MapFrom(s => s.Author.DisplayName))
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : null))
            .ForMember(d => d.CategorySlug, o => o.MapFrom(s => s.Category != null ? s.Category.Slug : null))
            .ForMember(d => d.Tags, o => o.MapFrom(s => s.PostTags.Select(pt => pt.Tag)));

        CreateMap<Post, PostDetailDto>()
            .IncludeBase<Post, PostListItemDto>()
            .ForMember(d => d.AuthorBio, o => o.MapFrom(s => s.Author.Bio))
            .ForMember(d => d.AuthorAvatarUrl, o => o.MapFrom(s => s.Author.AvatarUrl));
    }
}
