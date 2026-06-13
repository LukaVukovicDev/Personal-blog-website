using Blog.Application.Categories.Dtos;

namespace Blog.Application.Categories;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken ct = default);

    Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default);
}
