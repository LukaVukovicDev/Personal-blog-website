using Blog.Application.Comments.Dtos;
using Blog.Application.Notifications.Dtos;

namespace Blog.Application.Common.Interfaces;

/// <summary>Apstrakcija nad SignalR-om — implementacija živi u Blog.Api (NotificationsHub).</summary>
public interface IRealtimeNotifier
{
    /// <summary>Šalje notifikaciju (npr. "neko je komentarisao tvoj post") konkretnom korisniku.</summary>
    Task NotifyUserAsync(Guid userId, NotificationDto notification, CancellationToken ct = default);

    /// <summary>Šalje novi/odobreni komentar svima koji prate post (live komentari).</summary>
    Task NotifyPostCommentAsync(Guid postId, CommentDto comment, CancellationToken ct = default);
}
