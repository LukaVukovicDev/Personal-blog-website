using Blog.Domain.Entities;
using Blog.Domain.Enums;

namespace Blog.Domain.Interfaces;

public interface IPostRepository
{
    /// <summary>Stranicirana lista uz filtere; vraća stavke i ukupan broj.</summary>
    Task<(IReadOnlyList<Post> Items, int TotalCount)> GetPagedAsync(
        string? search,
        string? categorySlug,
        string? tagSlug,
        PostStatus? status,
        Guid? authorId,
        int page,
        int pageSize,
        CancellationToken ct = default);

    /// <summary>Učitava post sa autorom, kategorijom i tagovima.</summary>
    Task<Post?> GetBySlugAsync(string slug, bool includeUnpublished, CancellationToken ct = default);

    Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken ct = default);

    Task AddAsync(Post post, CancellationToken ct = default);

    void Update(Post post);

    void Remove(Post post);

    Task SaveChangesAsync(CancellationToken ct = default);
}
