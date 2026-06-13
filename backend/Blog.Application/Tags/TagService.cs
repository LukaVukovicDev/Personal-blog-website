using AutoMapper;
using Blog.Application.Tags.Dtos;
using Blog.Domain.Interfaces;

namespace Blog.Application.Tags;

public class TagService : ITagService
{
    private readonly ITagRepository tags;
    private readonly IMapper mapper;

    public TagService(ITagRepository tags, IMapper mapper)
    {
        this.tags = tags;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyList<TagDto>> GetAllAsync(CancellationToken ct = default) =>
        mapper.Map<List<TagDto>>(await tags.GetAllAsync(ct));
}
