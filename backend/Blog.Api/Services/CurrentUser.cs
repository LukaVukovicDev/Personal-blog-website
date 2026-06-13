using System.Security.Claims;
using Blog.Application.Common.Interfaces;

namespace Blog.Api.Services;

/// <summary>Čita identitet trenutnog korisnika iz JWT claim-ova (HttpContext).</summary>
public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor accessor;

    public CurrentUser(IHttpContextAccessor accessor)
    {
        this.accessor = accessor;
    }

    public Guid? UserId
    {
        get
        {
            var principal = accessor.HttpContext?.User;
            var value = principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal?.FindFirstValue("sub");

            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public bool IsAuthenticated => accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) => accessor.HttpContext?.User.IsInRole(role) ?? false;
}
