using Ganss.Xss;

namespace Blog.Application.Common.Utilities;

/// <summary>
/// Čisti HTML sadržaj postova pre čuvanja u bazi — uklanja &lt;script&gt;, event handlere (onerror, onload...),
/// "javascript:"/"data:" URI-je i ostale XSS vektore, dok zadržava formatiranje (naslovi, liste, kod, slike, linkovi...).
/// Sprečava stored XSS čak i ako autorov nalog bude kompromitovan.
/// </summary>
public static class PostContentSanitizer
{
    private static readonly HtmlSanitizer Sanitizer = CreateSanitizer();

    public static string Sanitize(string html) => Sanitizer.Sanitize(html);

    private static HtmlSanitizer CreateSanitizer()
    {
        var sanitizer = new HtmlSanitizer();

        sanitizer.AllowedTags.UnionWith(
        [
            "h1", "h2", "h3", "h4", "h5", "h6",
            "p", "br", "hr", "span", "div",
            "strong", "em", "b", "i", "u", "s", "mark", "small", "sub", "sup",
            "ul", "ol", "li",
            "blockquote", "pre", "code",
            "a", "img", "figure", "figcaption",
            "table", "thead", "tbody", "tfoot", "tr", "th", "td",
        ]);

        sanitizer.AllowedAttributes.UnionWith(["href", "src", "alt", "title", "class", "target", "rel"]);
        sanitizer.AllowedSchemes.UnionWith(["http", "https", "mailto"]);

        // "target=_blank" linkovi dobijaju rel="noopener noreferrer" da spreče reverse tabnabbing.
        sanitizer.PostProcessNode += (_, args) =>
        {
            if (args.Node is AngleSharp.Dom.IElement { TagName: "A" } anchor &&
                anchor.GetAttribute("target") == "_blank")
            {
                anchor.SetAttribute("rel", "noopener noreferrer");
            }
        };

        return sanitizer;
    }
}
