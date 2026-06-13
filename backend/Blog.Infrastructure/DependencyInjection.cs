using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Settings;
using Blog.Domain.Interfaces;
using Blog.Infrastructure.Authentication;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure;

/// <summary>
/// Registruje sve servise Infrastructure sloja (EF Core / MS SQL Server, repozitorijumi,
/// JWT, hashovanje lozinki). Poziva se kao <c>builder.Services.AddInfrastructure(config)</c>.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' nije pronađen (vidi appsettings.Local.json).");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
                // LocalDB se ugasi posle neaktivnosti — ponovi prolazne greške umesto pada.
                sql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        // Konfiguracija (Jwt sekcija → JwtSettings)
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        // Autentifikacija
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();

        // Repozitorijumi
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
}
