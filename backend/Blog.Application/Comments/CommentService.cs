using AutoMapper;
using Blog.Application.Comments.Dtos;
using Blog.Application.Common.Interfaces;
using Blog.Application.Notifications;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces;
using FluentValidation;

namespace Blog.Application.Comments;

public class CommentService : ICommentService
{
    private readonly ICommentRepository comments;
    private readonly IPostRepository posts;
    private readonly INotificationService notifications;
    private readonly IRealtimeNotifier realtime;
    private readonly IMapper mapper;
    private readonly IValidator<CreateCommentRequest> createValidator;

    public CommentService(
        ICommentRepository comments,
        IPostRepository posts,
        INotificationService notifications,
        IRealtimeNotifier realtime,
        IMapper mapper,
        IValidator<CreateCommentRequest> createValidator)
    {
        this.comments = comments;
        this.posts = posts;
        this.notifications = notifications;
        this.realtime = realtime;
        this.mapper = mapper;
        this.createValidator = createValidator;
    }

    public async Task<IReadOnlyList<CommentDto>> GetForPostAsync(Guid postId, bool includeAll, CancellationToken ct = default)
    {
        var list = await comments.GetByPostAsync(postId, approvedOnly: !includeAll, ct);
        return mapper.Map<List<CommentDto>>(list);
    }

    public async Task<IReadOnlyList<CommentAdminDto>> GetAllAsync(CommentStatus? status, CancellationToken ct = default)
    {
        var list = await comments.GetAllAsync(status, ct);
        return mapper.Map<List<CommentAdminDto>>(list);
    }

    public async Task<CommentDto> AddAsync(Guid postId, CreateCommentRequest request, Guid authorId, CancellationToken ct = default)
    {
        await createValidator.ValidateAndThrowAsync(request, ct);

        var post = await posts.GetByIdAsync(postId, ct)
            ?? throw new KeyNotFoundException("Post nije pronađen.");

        var comment = new Comment
        {
            PostId = post.Id,
            AuthorId = authorId,
            Body = request.Body.Trim(),
            ParentCommentId = request.ParentCommentId,
            Status = CommentStatus.Pending,
        };

        await comments.AddAsync(comment, ct);
        await comments.SaveChangesAsync(ct);

        // Ponovo učitaj sa autorom radi mapiranja AuthorName.
        var saved = await comments.GetByIdAsync(comment.Id, ct)
            ?? throw new InvalidOperationException("Komentar nije sačuvan.");

        // Obavesti autora posta (ako ne komentariše sam sebi).
        if (post.AuthorId != authorId)
        {
            await notifications.CreateAsync(
                post.AuthorId,
                NotificationType.NewComment,
                $"{saved.Author.DisplayName} je komentarisao tvoj post „{post.Title}“.",
                $"/post/{post.Slug}",
                ct);
        }

        return mapper.Map<CommentDto>(saved);
    }

    public async Task ModerateAsync(Guid commentId, CommentStatus status, CancellationToken ct = default)
    {
        var comment = await comments.GetByIdAsync(commentId, ct)
            ?? throw new KeyNotFoundException("Komentar nije pronađen.");

        comment.Status = status;
        await comments.SaveChangesAsync(ct);

        // Live update — kad admin odobri komentar, odmah ga prikaži svima na postu.
        if (status == CommentStatus.Approved)
        {
            await realtime.NotifyPostCommentAsync(comment.PostId, mapper.Map<CommentDto>(comment), ct);
        }
    }

    public async Task DeleteAsync(Guid commentId, CancellationToken ct = default)
    {
        var comment = await comments.GetByIdAsync(commentId, ct)
            ?? throw new KeyNotFoundException("Komentar nije pronađen.");

        comments.Remove(comment);
        await comments.SaveChangesAsync(ct);
    }
}
