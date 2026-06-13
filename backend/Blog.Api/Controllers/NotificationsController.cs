using Blog.Application.Common.Interfaces;
using Blog.Application.Notifications;
using Blog.Application.Notifications.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService notifications;
    private readonly ICurrentUser currentUser;

    public NotificationsController(INotificationService notifications, ICurrentUser currentUser)
    {
        this.notifications = notifications;
        this.currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NotificationDto>>> GetAll(CancellationToken ct) =>
        Ok(await notifications.GetForUserAsync(currentUser.UserId!.Value, ct));

    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount(CancellationToken ct) =>
        Ok(await notifications.GetUnreadCountAsync(currentUser.UserId!.Value, ct));

    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken ct)
    {
        await notifications.MarkAllAsReadAsync(currentUser.UserId!.Value, ct);
        return NoContent();
    }
}
