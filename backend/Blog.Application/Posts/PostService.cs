using System.Text.RegularExpressions;
using AutoMapper;
using Blog.Application.Common.Exceptions;
using Blog.Application.Common.Models;
using Blog.Application.Common.Utilities;
using Blog.Application.Posts.Dtos;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces;
using FluentValidation;

namespace Blog.Application.Posts;

public partial class PostService : IPostService
{
    private const int MaxPageSize = 50;

    private readonly IPostRepository posts;
    private readonly ITagRepository tags;
    private readonly IMapper mapper;
    private readonly IValidator<CreatePostRequest> createValidator;
    private readonly IValidator<UpdatePostRequest> updateValidator;

    public PostService(
        IPostRepository posts,
        ITagRepository tags,
        IMapper mapper,
        IValidator<CreatePostRequest> createValidator,
        IValidator<UpdatePostRequest> updateValidator)
    {
        this.posts = posts;
        this.tags = tags;
        this.mapper = mapper;
        this.createValidator = createValidator;
        this.updateValidator = updateValidator;
    }

    public Task<PagedResult<PostListItemDto>> GetPublishedAsync(PostQueryParameters query, CancellationToken ct = default) =>
        GetPagedAsync(query, PostStatus.Published, authorId: null, ct);

    public Task<PagedResult<PostListItemDto>> GetForManagementAsync(PostQueryParameters query, Guid currentUserId, bool isAdmin, CancellationToken ct = default) =>
        GetPagedAsync(query, status: null, authorId: isAdmin ? null : currentUserId, ct);

    public async Task<PostDetailDto> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var post = await posts.GetBySlugAsync(slug, includeUnpublished: false, ct)
            ?? throw new KeyNotFoundException("Post nije pronađen.");

        post.ViewCount++;
        await posts.SaveChangesAsync(ct);

        return mapper.Map<PostDetailDto>(post);
    }

    public async Task<PostDetailDto> GetByIdAsync(Guid id, Guid currentUserId, bool isAdmin, CancellationToken ct = default)
    {
        var post = await posts.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Post nije pronađen.");

        EnsureOwnership(post, currentUserId, isAdmin);

        return mapper.Map<PostDetailDto>(post);
    }

    public async Task<PostDetailDto> CreateAsync(CreatePostRequest request, Guid authorId, CancellationToken ct = default)
    {
        await createValidator.ValidateAndThrowAsync(request, ct);

        var contentHtml = PostContentSanitizer.Sanitize(request.ContentHtml);

        var post = new Post
        {
            Slug = await GenerateUniqueSlugAsync(request.Title, excludeId: null, ct),
            Title = request.Title.Trim(),
            Excerpt = request.Excerpt.Trim(),
            ContentHtml = contentHtml,
            CoverImageUrl = request.CoverImageUrl,
            CategoryId = request.CategoryId,
            AuthorId = authorId,
            ReadMinutes = EstimateReadMinutes(contentHtml),
            Status = request.Publish ? PostStatus.Published : PostStatus.Draft,
            PublishedAt = request.Publish ? DateTime.UtcNow : null,
        };

        foreach (var tag in await ResolveTagsAsync(request.TagNames, ct))
        {
            post.PostTags.Add(new PostTag { Tag = tag });
        }

        await posts.AddAsync(post, ct);
        await posts.SaveChangesAsync(ct);

        return await LoadDetailAsync(post.Id, ct);
    }

    public async Task<PostDetailDto> UpdateAsync(Guid id, UpdatePostRequest request, Guid currentUserId, bool isAdmin, CancellationToken ct = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, ct);

        var post = await posts.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Post nije pronađen.");

        EnsureOwnership(post, currentUserId, isAdmin);

        post.Title = request.Title.Trim();
        post.Excerpt = request.Excerpt.Trim();
        post.ContentHtml = PostContentSanitizer.Sanitize(request.ContentHtml);
        post.CoverImageUrl = request.CoverImageUrl;
        post.CategoryId = request.CategoryId;
        post.ReadMinutes = EstimateReadMinutes(post.ContentHtml);
        post.UpdatedAt = DateTime.UtcNow;

        post.PostTags.Clear();
        foreach (var tag in await ResolveTagsAsync(request.TagNames, ct))
        {
            post.PostTags.Add(new PostTag { Tag = tag });
        }

        await posts.SaveChangesAsync(ct);

        return await LoadDetailAsync(post.Id, ct);
    }

    public async Task PublishAsync(Guid id, Guid currentUserId, bool isAdmin, CancellationToken ct = default)
    {
        var post = await posts.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Post nije pronađen.");

        EnsureOwnership(post, currentUserId, isAdmin);

        if (post.Status != PostStatus.Published)
        {
            post.Status = PostStatus.Published;
            post.PublishedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            await posts.SaveChangesAsync(ct);
        }
    }

    public async Task DeleteAsync(Guid id, Guid currentUserId, bool isAdmin, CancellationToken ct = default)
    {
        var post = await posts.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Post nije pronađen.");

        EnsureOwnership(post, currentUserId, isAdmin);

        posts.Remove(post);
        await posts.SaveChangesAsync(ct);
    }

    /// <summary>Admin sme sve; Author sme samo nad sopstvenim postom — inače 401/403.</summary>
    private static void EnsureOwnership(Post post, Guid currentUserId, bool isAdmin)
    {
        if (!isAdmin && post.AuthorId != currentUserId)
        {
            throw new ForbiddenAccessException("Nemate dozvolu da upravljate ovim postom.");
        }
    }

    private async Task<PagedResult<PostListItemDto>> GetPagedAsync(
        PostQueryParameters query, PostStatus? status, Guid? authorId, CancellationToken ct)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var size = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var (items, total) = await posts.GetPagedAsync(
            query.Search, query.Category, query.Tag, status, authorId, page, size, ct);

        var dtos = mapper.Map<List<PostListItemDto>>(items);
        return new PagedResult<PostListItemDto>(dtos, page, size, total);
    }

    private async Task<PostDetailDto> LoadDetailAsync(Guid id, CancellationToken ct)
    {
        var post = await posts.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Post nije pronađen.");

        return mapper.Map<PostDetailDto>(post);
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, Guid? excludeId, CancellationToken ct)
    {
        var baseSlug = Slug.Generate(title);
        var slug = baseSlug;
        var suffix = 2;

        while (await posts.SlugExistsAsync(slug, excludeId, ct))
        {
            slug = $"{baseSlug}-{suffix++}";
        }

        return slug;
    }

    private async Task<List<Tag>> ResolveTagsAsync(IEnumerable<string> tagNames, CancellationToken ct)
    {
        var requested = tagNames
            .Select(name => name.Trim())
            .Where(name => name.Length > 0)
            .Select(name => new { Name = name, Slug = Slug.Generate(name) })
            .GroupBy(x => x.Slug)
            .Select(group => group.First())
            .ToList();

        if (requested.Count == 0)
        {
            return [];
        }

        var existing = (await tags.GetBySlugsAsync(requested.Select(r => r.Slug), ct)).ToList();
        var result = new List<Tag>(existing);

        foreach (var item in requested.Where(item => existing.All(t => t.Slug != item.Slug)))
        {
            // Nov tag — EF ga ubacuje kroz graf prilikom čuvanja posta.
            result.Add(new Tag { Name = item.Name, Slug = item.Slug });
        }

        return result;
    }

    private static int EstimateReadMinutes(string html)
    {
        var text = HtmlTags().Replace(html, " ");
        var words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Max(1, (int)Math.Ceiling(words / 200.0));
    }

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex HtmlTags();
}
