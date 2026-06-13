using Blog.Application.Comments.Dtos;
using Blog.Domain.Enums;

namespace Blog.Application.Comments;

public interface ICommentService
{
    /// <summary><paramref name="includeAll"/>=false vraća samo odobrene (javni prikaz).</summary>
    Task<IReadOnlyList<CommentDto>> GetForPostAsync(Guid postId, bool includeAll, CancellationToken ct = default);

    /// <summary>Svi komentari za admin moderaciju, opciono filtrirani po statusu.</summary>
    Task<IReadOnlyList<CommentAdminDto>> GetAllAsync(CommentStatus? status, CancellationToken ct = default);

    Task<CommentDto> AddAsync(Guid postId, CreateCommentRequest request, Guid authorId, CancellationToken ct = default);

    Task ModerateAsync(Guid commentId, CommentStatus status, CancellationToken ct = default);

    Task DeleteAsync(Guid commentId, CancellationToken ct = default);
}
