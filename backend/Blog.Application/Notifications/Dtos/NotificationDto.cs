namespace Blog.Application.Notifications.Dtos;

public class NotificationDto
{
    public Guid Id { get; init; }

    public string Type { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public string? LinkUrl { get; init; }

    public bool IsRead { get; init; }

    public DateTime CreatedAt { get; init; }
}
