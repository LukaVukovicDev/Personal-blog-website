using AutoMapper;
using Blog.Application.Common.Interfaces;
using Blog.Application.Notifications.Dtos;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces;

namespace Blog.Application.Notifications;

public class NotificationService : INotificationService
{
    private const int MaxResults = 20;

    private readonly INotificationRepository notifications;
    private readonly IRealtimeNotifier realtime;
    private readonly IMapper mapper;

    public NotificationService(INotificationRepository notifications, IRealtimeNotifier realtime, IMapper mapper)
    {
        this.notifications = notifications;
        this.realtime = realtime;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyList<NotificationDto>> GetForUserAsync(Guid userId, CancellationToken ct = default)
    {
        var list = await notifications.GetForUserAsync(userId, MaxResults, ct);
        return mapper.Map<List<NotificationDto>>(list);
    }

    public Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default) =>
        notifications.GetUnreadCountAsync(userId, ct);

    public Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default) =>
        notifications.MarkAllAsReadAsync(userId, ct);

    public async Task<NotificationDto> CreateAsync(
        Guid userId, NotificationType type, string message, string? linkUrl, CancellationToken ct = default)
    {
        var notification = new Notification
        {
            UserId = userId,
            Type = type,
            Message = message,
            LinkUrl = linkUrl,
        };

        await notifications.AddAsync(notification, ct);
        await notifications.SaveChangesAsync(ct);

        var dto = mapper.Map<NotificationDto>(notification);
        await realtime.NotifyUserAsync(userId, dto, ct);

        return dto;
    }
}
