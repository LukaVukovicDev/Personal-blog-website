using AutoMapper;
using Blog.Application.Categories.Dtos;
using Blog.Domain.Entities;

namespace Blog.Application.Categories.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}
