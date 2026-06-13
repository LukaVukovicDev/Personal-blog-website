namespace Blog.Application.Common.Settings;

/// <summary>Vrednosti iz <c>Jwt</c> sekcije konfiguracije (appsettings + appsettings.Local.json).</summary>
public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    /// <summary>Tajni ključ za potpisivanje (HMAC-SHA256, min. 32 karaktera). Drži se van git-a.</summary>
    public string Secret { get; set; } = string.Empty;

    public int AccessTokenMinutes { get; set; } = 15;

    public int RefreshTokenDays { get; set; } = 7;
}
