using Blog.Domain.Common;
using Blog.Domain.Enums;

namespace Blog.Domain.Entities;

public class Comment : BaseEntity
{
    public Guid PostId { get; set; }

    public Post Post { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public User Author { get; set; } = null!;

    public string Body { get; set; } = string.Empty;

    public CommentStatus Status { get; set; } = CommentStatus.Pending;

    // Odgovor na drugi komentar (opciono, za niti)
    public Guid? ParentCommentId { get; set; }

    public Comment? ParentComment { get; set; }

    public ICollection<Comment> Replies { get; set; } = new List<Comment>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
