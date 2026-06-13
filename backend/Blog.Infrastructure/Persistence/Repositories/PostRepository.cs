using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext context;

    public PostRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<(IReadOnlyList<Post> Items, int TotalCount)> GetPagedAsync(
        string? search,
        string? categorySlug,
        string? tagSlug,
        PostStatus? status,
        Guid? authorId,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = context.Posts
            .AsNoTracking()
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
            .AsQueryable();

        if (status is not null)
        {
            query = query.Where(p => p.Status == status);
        }

        if (authorId is not null)
        {
            query = query.Where(p => p.AuthorId == authorId);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p => p.Title.Contains(term) || p.Excerpt.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            query = query.Where(p => p.Category != null && p.Category.Slug == categorySlug);
        }

        if (!string.IsNullOrWhiteSpace(tagSlug))
        {
            query = query.Where(p => p.PostTags.Any(pt => pt.Tag.Slug == tagSlug));
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.PublishedAt ?? p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<Post?> GetBySlugAsync(string slug, bool includeUnpublished, CancellationToken ct = default)
    {
        var query = context.Posts
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
            .AsQueryable();

        if (!includeUnpublished)
        {
            query = query.Where(p => p.Status == PostStatus.Published);
        }

        return query.FirstOrDefaultAsync(p => p.Slug == slug, ct);
    }

    public Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Posts
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken ct = default) =>
        context.Posts.AnyAsync(p => p.Slug == slug && (excludeId == null || p.Id != excludeId), ct);

    public async Task AddAsync(Post post, CancellationToken ct = default) =>
        await context.Posts.AddAsync(post, ct);

    public void Update(Post post) => context.Posts.Update(post);

    public void Remove(Post post) => context.Posts.Remove(post);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
