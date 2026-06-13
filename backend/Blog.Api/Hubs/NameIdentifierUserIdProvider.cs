using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Api.Hubs;

/// <summary>Identifikuje SignalR korisnika po "sub" claim-u iz JWT-a (vidi JwtService).</summary>
public class NameIdentifierUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection) =>
        connection.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
}
