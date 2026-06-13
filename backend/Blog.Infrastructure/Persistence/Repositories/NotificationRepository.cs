using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext context;

    public NotificationRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<Notification>> GetForUserAsync(Guid userId, int take, CancellationToken ct = default) =>
        await context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(take)
            .ToListAsync(ct);

    public Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default) =>
        context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead, ct);

    public Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Notifications.FirstOrDefaultAsync(n => n.Id == id, ct);

    public async Task AddAsync(Notification notification, CancellationToken ct = default) =>
        await context.Notifications.AddAsync(notification, ct);

    public async Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default)
    {
        await context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(setters => setters.SetProperty(n => n.IsRead, true), ct);
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
