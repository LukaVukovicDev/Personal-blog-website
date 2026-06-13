using Blog.Domain.Common;
using Blog.Domain.Enums;

namespace Blog.Domain.Entities;

public class Post : BaseEntity
{
    public string Slug { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Excerpt { get; set; } = string.Empty;

    public string ContentHtml { get; set; } = string.Empty;

    public string? CoverImageUrl { get; set; }

    public PostStatus Status { get; set; } = PostStatus.Draft;

    public int ReadMinutes { get; set; }

    public int ViewCount { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Autor
    public Guid AuthorId { get; set; }

    public User Author { get; set; } = null!;

    // Kategorija (opciona)
    public Guid? CategoryId { get; set; }

    public Category? Category { get; set; }

    // Navigacije
    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
