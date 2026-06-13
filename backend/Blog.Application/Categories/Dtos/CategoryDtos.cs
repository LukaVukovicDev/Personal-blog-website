namespace Blog.Application.Categories.Dtos;

public class CategoryDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string? Description { get; init; }
}

public record CreateCategoryRequest(string Name, string? Description);
