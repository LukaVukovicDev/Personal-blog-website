using Blog.Application.Comments;
using Blog.Application.Comments.Dtos;
using Blog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/comments")]
[Authorize(Roles = "Admin")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService comments;

    public CommentsController(ICommentService comments)
    {
        this.comments = comments;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CommentAdminDto>>> GetAll(
        [FromQuery] CommentStatus? status, CancellationToken ct) =>
        Ok(await comments.GetAllAsync(status, ct));

    [HttpPut("{id:guid}/moderate")]
    public async Task<IActionResult> Moderate(Guid id, ModerateCommentRequest request, CancellationToken ct)
    {
        await comments.ModerateAsync(id, request.Status, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await comments.DeleteAsync(id, ct);
        return NoContent();
    }
}
