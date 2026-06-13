using Blog.Domain.Entities;
using Blog.Domain.Enums;

namespace Blog.Domain.Interfaces;

public interface ICommentRepository
{
    /// <summary>Komentari jednog posta sa autorom; <paramref name="approvedOnly"/> filtrira odobrene.</summary>
    Task<IReadOnlyList<Comment>> GetByPostAsync(Guid postId, bool approvedOnly, CancellationToken ct = default);

    /// <summary>Svi komentari (sa autorom i postom) za admin moderaciju; opciono filtrirano po statusu.</summary>
    Task<IReadOnlyList<Comment>> GetAllAsync(CommentStatus? status, CancellationToken ct = default);

    Task<Comment?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(Comment comment, CancellationToken ct = default);

    void Update(Comment comment);

    void Remove(Comment comment);

    Task SaveChangesAsync(CancellationToken ct = default);
}
