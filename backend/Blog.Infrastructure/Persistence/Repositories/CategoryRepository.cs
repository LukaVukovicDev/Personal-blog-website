using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext context;

    public CategoryRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default) =>
        await context.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(ct);

    public Task<Category?> GetBySlugAsync(string slug, CancellationToken ct = default) =>
        context.Categories.FirstOrDefaultAsync(c => c.Slug == slug, ct);

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default) =>
        context.Categories.AnyAsync(c => c.Slug == slug, ct);

    public async Task AddAsync(Category category, CancellationToken ct = default) =>
        await context.Categories.AddAsync(category, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
