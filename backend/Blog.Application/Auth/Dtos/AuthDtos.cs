namespace Blog.Application.Auth.Dtos;

public record RegisterRequest(string Username, string Email, string Password, string DisplayName);

public record LoginRequest(string UsernameOrEmail, string Password);

public record RefreshRequest(string RefreshToken);

public record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto User);

public class UserDto
{
    public Guid Id { get; init; }

    public string Username { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string DisplayName { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;

    public string? Bio { get; init; }

    public string? AvatarUrl { get; init; }
}
