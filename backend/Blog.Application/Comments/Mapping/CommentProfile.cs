using AutoMapper;
using Blog.Application.Comments.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Comments.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.AuthorName, o => o.MapFrom(s => s.Author.DisplayName));

        CreateMap<Comment, CommentAdminDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.AuthorName, o => o.MapFrom(s => s.Author.DisplayName))
            .ForMember(d => d.PostTitle, o => o.MapFrom(s => s.Post.Title))
            .ForMember(d => d.PostSlug, o => o.MapFrom(s => s.Post.Slug));
    }
}
