using Blog.Application.Notifications.Dtos;
using Blog.Domain.Enums;

namespace Blog.Application.Notifications;

public interface INotificationService
{
    /// <summary>Poslednjih 20 notifikacija korisnika, najnovije prve.</summary>
    Task<IReadOnlyList<NotificationDto>> GetForUserAsync(Guid userId, CancellationToken ct = default);

    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);

    Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default);

    /// <summary>Kreira i perzistira notifikaciju, pa je odmah šalje preko SignalR-a (ako je korisnik povezan).</summary>
    Task<NotificationDto> CreateAsync(
        Guid userId, NotificationType type, string message, string? linkUrl, CancellationToken ct = default);
}
