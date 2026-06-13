using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext context;

    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken ct = default) =>
        context.Users.FirstOrDefaultAsync(
            u => u.Email == usernameOrEmail || u.Username == usernameOrEmail, ct);

    public Task<bool> ExistsAsync(string email, string username, CancellationToken ct = default) =>
        context.Users.AnyAsync(u => u.Email == email || u.Username == username, ct);

    public async Task AddAsync(User user, CancellationToken ct = default) =>
        await context.Users.AddAsync(user, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
