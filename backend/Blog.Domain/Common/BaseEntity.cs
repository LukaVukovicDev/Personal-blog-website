namespace Blog.Domain.Common;

/// <summary>Zajednička osnova svih entiteta — Guid primarni ključ.</summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
