using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default);

    Task<Category?> GetBySlugAsync(string slug, CancellationToken ct = default);

    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default);

    Task AddAsync(Category category, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
