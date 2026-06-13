using Microsoft.AspNetCore.SignalR;

namespace Blog.Api.Hubs;

/// <summary>
/// Hub za real-time notifikacije i live komentare. Anonimni klijenti mogu da se povežu
/// (da bi pratili grupu posta za live komentare); samo prijavljeni korisnici imaju
/// <see cref="HubCallerContext.UserIdentifier"/> i mogu da prime lične notifikacije.
/// </summary>
public class NotificationsHub : Hub
{
    private static string PostGroup(Guid postId) => $"post-{postId}";

    /// <summary>Klijent se pridružuje grupi posta da bi primao live komentare na njemu.</summary>
    public Task JoinPostGroup(Guid postId) =>
        Groups.AddToGroupAsync(Context.ConnectionId, PostGroup(postId));

    public Task LeavePostGroup(Guid postId) =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, PostGroup(postId));
}
