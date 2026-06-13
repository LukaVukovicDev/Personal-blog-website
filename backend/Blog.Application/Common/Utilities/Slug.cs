using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Application.Common.Utilities;

/// <summary>Pravi URL-friendly "slug" od proizvoljnog teksta (npr. naslova posta).</summary>
public static partial class Slug
{
    public static string Generate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Guid.NewGuid().ToString("n")[..8];
        }

        var lower = input.Trim().ToLowerInvariant();
        var builder = new StringBuilder(lower.Length);

        foreach (var ch in lower)
        {
            if (char.IsLetterOrDigit(ch))
            {
                builder.Append(ch);
            }
            else if (char.IsWhiteSpace(ch) || ch is '-' or '_')
            {
                builder.Append('-');
            }
        }

        var collapsed = MultipleDashes().Replace(builder.ToString(), "-").Trim('-');
        return collapsed.Length == 0 ? Guid.NewGuid().ToString("n")[..8] : collapsed;
    }

    [GeneratedRegex("-{2,}")]
    private static partial Regex MultipleDashes();
}
