using Blog.Application.Comments.Dtos;
using Blog.Application.Common.Interfaces;
using Blog.Application.Notifications.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Api.Hubs;

/// <summary>Implementacija <see cref="IRealtimeNotifier"/> preko <see cref="NotificationsHub"/>.</summary>
public class SignalRNotifier : IRealtimeNotifier
{
    private readonly IHubContext<NotificationsHub> hub;

    public SignalRNotifier(IHubContext<NotificationsHub> hub)
    {
        this.hub = hub;
    }

    public Task NotifyUserAsync(Guid userId, NotificationDto notification, CancellationToken ct = default) =>
        hub.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification, ct);

    public Task NotifyPostCommentAsync(Guid postId, CommentDto comment, CancellationToken ct = default) =>
        hub.Clients.Group($"post-{postId}").SendAsync("CommentAdded", comment, ct);
}
