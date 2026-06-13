using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces;

public interface INotificationRepository
{
    /// <summary>Notifikacije korisnika, najnovije prve.</summary>
    Task<IReadOnlyList<Notification>> GetForUserAsync(Guid userId, int take, CancellationToken ct = default);

    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);

    Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(Notification notification, CancellationToken ct = default);

    Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
