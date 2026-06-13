using Blog.Application.Tags;
using Blog.Application.Tags.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController : ControllerBase
{
    private readonly ITagService tags;

    public TagsController(ITagService tags)
    {
        this.tags = tags;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TagDto>>> GetAll(CancellationToken ct) =>
        Ok(await tags.GetAllAsync(ct));
}
