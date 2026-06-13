namespace Blog.Application.Common.Exceptions;

/// <summary>Korisnik je autentifikovan, ali nema dozvolu za traženu radnju (mapira se na 403).</summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }
}
