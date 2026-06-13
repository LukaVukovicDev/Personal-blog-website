using Blog.Application.Comments;
using Blog.Application.Comments.Dtos;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Models;
using Blog.Application.Posts;
using Blog.Application.Posts.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private const string AuthorOrAdmin = "Author,Admin";

    private readonly IPostService posts;
    private readonly ICommentService comments;
    private readonly ICurrentUser currentUser;

    public PostsController(IPostService posts, ICommentService comments, ICurrentUser currentUser)
    {
        this.posts = posts;
        this.comments = comments;
        this.currentUser = currentUser;
    }

    // ── Javno ────────────────────────────────────────────────────────────────────

    [HttpGet]
    public async Task<ActionResult<PagedResult<PostListItemDto>>> GetList(
        [FromQuery] PostQueryParameters query, CancellationToken ct) =>
        Ok(await posts.GetPublishedAsync(query, ct));

    [HttpGet("{slug}")]
    public async Task<ActionResult<PostDetailDto>> GetBySlug(string slug, CancellationToken ct) =>
        Ok(await posts.GetBySlugAsync(slug, ct));

    [HttpGet("{postId:guid}/comments")]
    public async Task<ActionResult<IReadOnlyList<CommentDto>>> GetComments(Guid postId, CancellationToken ct) =>
        Ok(await comments.GetForPostAsync(postId, includeAll: false, ct));

    [Authorize]
    [HttpPost("{postId:guid}/comments")]
    public async Task<ActionResult<CommentDto>> AddComment(
        Guid postId, CreateCommentRequest request, CancellationToken ct)
    {
        var authorId = currentUser.UserId ?? throw new UnauthorizedAccessException();
        return Ok(await comments.AddAsync(postId, request, authorId, ct));
    }

    // ── Upravljanje (Author/Admin) ───────────────────────────────────────────────

    [Authorize(Roles = AuthorOrAdmin)]
    [HttpGet("manage")]
    public async Task<ActionResult<PagedResult<PostListItemDto>>> GetManaged(
        [FromQuery] PostQueryParameters query, CancellationToken ct)
    {
        var (userId, isAdmin) = CurrentUserContext();
        return Ok(await posts.GetForManagementAsync(query, userId, isAdmin, ct));
    }

    [Authorize(Roles = AuthorOrAdmin)]
    [HttpGet("manage/{id:guid}")]
    public async Task<ActionResult<PostDetailDto>> GetById(Guid id, CancellationToken ct)
    {
        var (userId, isAdmin) = CurrentUserContext();
        return Ok(await posts.GetByIdAsync(id, userId, isAdmin, ct));
    }

    [Authorize(Roles = AuthorOrAdmin)]
    [HttpPost]
    public async Task<ActionResult<PostDetailDto>> Create(CreatePostRequest request, CancellationToken ct)
    {
        var authorId = currentUser.UserId ?? throw new UnauthorizedAccessException();
        var created = await posts.CreateAsync(request, authorId, ct);
        return CreatedAtAction(nameof(GetBySlug), new { slug = created.Slug }, created);
    }

    [Authorize(Roles = AuthorOrAdmin)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PostDetailDto>> Update(
        Guid id, UpdatePostRequest request, CancellationToken ct)
    {
        var (userId, isAdmin) = CurrentUserContext();
        return Ok(await posts.UpdateAsync(id, request, userId, isAdmin, ct));
    }

    [Authorize(Roles = AuthorOrAdmin)]
    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
    {
        var (userId, isAdmin) = CurrentUserContext();
        await posts.PublishAsync(id, userId, isAdmin, ct);
        return NoContent();
    }

    [Authorize(Roles = AuthorOrAdmin)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var (userId, isAdmin) = CurrentUserContext();
        await posts.DeleteAsync(id, userId, isAdmin, ct);
        return NoContent();
    }

    private (Guid UserId, bool IsAdmin) CurrentUserContext()
    {
        var userId = currentUser.UserId ?? throw new UnauthorizedAccessException();
        return (userId, currentUser.IsInRole("Admin"));
    }
}
