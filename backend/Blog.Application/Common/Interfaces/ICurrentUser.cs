namespace Blog.Application.Common.Interfaces;

/// <summary>Apstrakcija nad trenutno prijavljenim korisnikom (čita se iz JWT claim-ova).</summary>
public interface ICurrentUser
{
    Guid? UserId { get; }

    bool IsAuthenticated { get; }

    bool IsInRole(string role);
}
