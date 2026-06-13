using Blog.Domain.Common;
using Blog.Domain.Enums;

namespace Blog.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }

    public UserRole Role { get; set; } = UserRole.Reader;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigacije
    public ICollection<Post> Posts { get; set; } = new List<Post>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
