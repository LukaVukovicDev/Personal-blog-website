using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext context;

    public TagRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<Tag>> GetAllAsync(CancellationToken ct = default) =>
        await context.Tags.AsNoTracking().OrderBy(t => t.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Tag>> GetBySlugsAsync(IEnumerable<string> slugs, CancellationToken ct = default)
    {
        var wanted = slugs.ToList();
        return await context.Tags.Where(t => wanted.Contains(t.Slug)).ToListAsync(ct);
    }

    public async Task AddRangeAsync(IEnumerable<Tag> tags, CancellationToken ct = default) =>
        await context.Tags.AddRangeAsync(tags, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
