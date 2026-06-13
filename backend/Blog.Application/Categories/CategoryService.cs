using AutoMapper;
using Blog.Application.Categories.Dtos;
using Blog.Application.Common.Utilities;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using FluentValidation;

namespace Blog.Application.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository categories;
    private readonly IMapper mapper;
    private readonly IValidator<CreateCategoryRequest> createValidator;

    public CategoryService(ICategoryRepository categories, IMapper mapper, IValidator<CreateCategoryRequest> createValidator)
    {
        this.categories = categories;
        this.mapper = mapper;
        this.createValidator = createValidator;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken ct = default) =>
        mapper.Map<List<CategoryDto>>(await categories.GetAllAsync(ct));

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default)
    {
        await createValidator.ValidateAndThrowAsync(request, ct);

        var slug = Slug.Generate(request.Name);

        if (await categories.SlugExistsAsync(slug, ct))
        {
            throw new InvalidOperationException("Kategorija sa tim nazivom već postoji.");
        }

        var category = new Category
        {
            Name = request.Name.Trim(),
            Slug = slug,
            Description = request.Description?.Trim(),
        };

        await categories.AddAsync(category, ct);
        await categories.SaveChangesAsync(ct);

        return mapper.Map<CategoryDto>(category);
    }
}
