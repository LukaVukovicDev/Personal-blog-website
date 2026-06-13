using Blog.Application.Tags.Dtos;

namespace Blog.Application.Tags;

public interface ITagService
{
    Task<IReadOnlyList<TagDto>> GetAllAsync(CancellationToken ct = default);
}
