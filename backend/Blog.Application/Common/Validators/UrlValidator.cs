namespace Blog.Application.Common.Validators;

/// <summary>Zajedničke provere za URL polja unetih od strane korisnika (sprečava "javascript:"/"data:" XSS vektore).</summary>
public static class UrlValidator
{
    /// <summary>Prazno/null je dozvoljeno (opciono polje); ako je uneto, mora biti http ili https URL.</summary>
    public static bool BeHttpUrlOrEmpty(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return true;
        }

        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
