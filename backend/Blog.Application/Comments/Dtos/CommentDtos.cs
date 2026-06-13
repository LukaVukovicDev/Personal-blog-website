using Blog.Domain.Enums;

namespace Blog.Application.Comments.Dtos;

public class CommentDto
{
    public Guid Id { get; init; }

    public string Body { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public Guid AuthorId { get; init; }

    public string AuthorName { get; init; } = string.Empty;

    public Guid? ParentCommentId { get; init; }
}

/// <summary>Komentar sa podacima o postu — koristi se za admin moderaciju (lista svih komentara).</summary>
public class CommentAdminDto : CommentDto
{
    public string PostTitle { get; init; } = string.Empty;

    public string PostSlug { get; init; } = string.Empty;
}

public record CreateCommentRequest(string Body, Guid? ParentCommentId);

public record ModerateCommentRequest(CommentStatus Status);
