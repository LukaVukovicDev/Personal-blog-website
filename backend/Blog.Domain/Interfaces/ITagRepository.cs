using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces;

public interface ITagRepository
{
    Task<IReadOnlyList<Tag>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Vraća postojeće tagove čiji su slug-ovi u prosleđenom skupu.</summary>
    Task<IReadOnlyList<Tag>> GetBySlugsAsync(IEnumerable<string> slugs, CancellationToken ct = default);

    Task AddRangeAsync(IEnumerable<Tag> tags, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
