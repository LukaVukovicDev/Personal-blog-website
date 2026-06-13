using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext context;

    public CommentRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<Comment>> GetByPostAsync(Guid postId, bool approvedOnly, CancellationToken ct = default)
    {
        var query = context.Comments
            .AsNoTracking()
            .Include(c => c.Author)
            .Where(c => c.PostId == postId);

        if (approvedOnly)
        {
            query = query.Where(c => c.Status == CommentStatus.Approved);
        }

        return await query.OrderBy(c => c.CreatedAt).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Comment>> GetAllAsync(CommentStatus? status, CancellationToken ct = default)
    {
        var query = context.Comments
            .AsNoTracking()
            .Include(c => c.Author)
            .Include(c => c.Post)
            .AsQueryable();

        if (status is not null)
        {
            query = query.Where(c => c.Status == status);
        }

        return await query.OrderByDescending(c => c.CreatedAt).ToListAsync(ct);
    }

    public Task<Comment?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Comments
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task AddAsync(Comment comment, CancellationToken ct = default) =>
        await context.Comments.AddAsync(comment, ct);

    public void Update(Comment comment) => context.Comments.Update(comment);

    public void Remove(Comment comment) => context.Comments.Remove(comment);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
