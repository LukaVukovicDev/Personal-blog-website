using System.Security.Cryptography;
using System.Text;

namespace Blog.Application.Common.Utilities;

/// <summary>
/// Refresh tokeni se u bazi čuvaju kao SHA-256 heš (ne kao plaintext), tako da kompromitovanje baze
/// samo po sebi ne daje napadaču upotrebljive sesijske tokene.
/// </summary>
public static class RefreshTokenHasher
{
    public static string Hash(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }
}
