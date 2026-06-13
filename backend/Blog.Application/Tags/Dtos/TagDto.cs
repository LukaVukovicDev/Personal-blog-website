namespace Blog.Application.Tags.Dtos;

public class TagDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;
}
