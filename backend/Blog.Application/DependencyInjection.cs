using System.Reflection;
using Blog.Application.Auth;
using Blog.Application.Categories;
using Blog.Application.Comments;
using Blog.Application.Notifications;
using Blog.Application.Posts;
using Blog.Application.Tags;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

/// <summary>
/// Registruje sve servise Application sloja (use-case logika, mapiranja).
/// Poziva se iz <c>Program.cs</c> kao <c>builder.Services.AddApplication()</c>.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = Assembly.GetExecutingAssembly();

        // AutoMapper pokupi sve Profile klase iz ovog assembly-ja.
        services.AddAutoMapper(cfg => cfg.AddMaps(applicationAssembly));

        // FluentValidation pokupi sve validatore iz ovog assembly-ja.
        services.AddValidatorsFromAssembly(applicationAssembly);

        // Aplikativni servisi
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
