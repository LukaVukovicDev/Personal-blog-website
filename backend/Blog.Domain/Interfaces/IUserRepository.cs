using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Login dozvoljava prijavu i preko username-a i preko email-a.</summary>
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken ct = default);

    Task<bool> ExistsAsync(string email, string username, CancellationToken ct = default);

    Task AddAsync(User user, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
