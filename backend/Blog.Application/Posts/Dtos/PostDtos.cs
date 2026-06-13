using Blog.Application.Tags.Dtos;

namespace Blog.Application.Posts.Dtos;

public class PostListItemDto
{
    public Guid Id { get; init; }

    public string Slug { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Excerpt { get; init; } = string.Empty;

    public string? CoverImageUrl { get; init; }

    public string Status { get; init; } = string.Empty;

    public int ReadMinutes { get; init; }

    public int ViewCount { get; init; }

    public DateTime? PublishedAt { get; init; }

    public DateTime CreatedAt { get; init; }

    public string AuthorName { get; init; } = string.Empty;

    public string? CategoryName { get; init; }

    public string? CategorySlug { get; init; }

    public List<TagDto> Tags { get; init; } = [];
}

public class PostDetailDto : PostListItemDto
{
    public string ContentHtml { get; init; } = string.Empty;

    public string? AuthorBio { get; init; }

    public string? AuthorAvatarUrl { get; init; }
}

public record CreatePostRequest(
    string Title,
    string Excerpt,
    string ContentHtml,
    string? CoverImageUrl,
    Guid? CategoryId,
    List<string> TagNames,
    bool Publish);

public record UpdatePostRequest(
    string Title,
    string Excerpt,
    string ContentHtml,
    string? CoverImageUrl,
    Guid? CategoryId,
    List<string> TagNames);

/// <summary>Parametri za filtriranje i straničenje liste postova (vezuju se iz query stringa).</summary>
public class PostQueryParameters
{
    public string? Search { get; init; }

    public string? Category { get; init; }

    public string? Tag { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 9;
}
