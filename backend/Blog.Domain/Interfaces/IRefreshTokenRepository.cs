using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    /// <summary>Vraća token zajedno sa povezanim korisnikom (Include User).</summary>
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);

    Task AddAsync(RefreshToken token, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
