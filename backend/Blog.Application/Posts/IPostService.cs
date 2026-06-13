using Blog.Application.Common.Models;
using Blog.Application.Posts.Dtos;

namespace Blog.Application.Posts;

public interface IPostService
{
    /// <summary>Javna lista — samo objavljeni postovi.</summary>
    Task<PagedResult<PostListItemDto>> GetPublishedAsync(PostQueryParameters query, CancellationToken ct = default);

    /// <summary>Lista za admin/author panel — Admin vidi sve, Author samo svoje postove (uključujući draftove).</summary>
    Task<PagedResult<PostListItemDto>> GetForManagementAsync(PostQueryParameters query, Guid currentUserId, bool isAdmin, CancellationToken ct = default);

    Task<PostDetailDto> GetBySlugAsync(string slug, CancellationToken ct = default);

    /// <summary>Detalj posta za upravljanje — Admin vidi svaki post, Author samo svoj.</summary>
    Task<PostDetailDto> GetByIdAsync(Guid id, Guid currentUserId, bool isAdmin, CancellationToken ct = default);

    Task<PostDetailDto> CreateAsync(CreatePostRequest request, Guid authorId, CancellationToken ct = default);

    /// <summary>Admin može da izmeni svaki post, Author samo svoj.</summary>
    Task<PostDetailDto> UpdateAsync(Guid id, UpdatePostRequest request, Guid currentUserId, bool isAdmin, CancellationToken ct = default);

    /// <summary>Admin može da objavi svaki post, Author samo svoj.</summary>
    Task PublishAsync(Guid id, Guid currentUserId, bool isAdmin, CancellationToken ct = default);

    /// <summary>Admin može da obriše svaki post, Author samo svoj.</summary>
    Task DeleteAsync(Guid id, Guid currentUserId, bool isAdmin, CancellationToken ct = default);
}
