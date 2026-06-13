using Blog.Domain.Common;
using Blog.Domain.Enums;

namespace Blog.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public NotificationType Type { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? LinkUrl { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
