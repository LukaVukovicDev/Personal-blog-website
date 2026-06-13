using System.Text.RegularExpressions;
using Blog.Application.Common.Utilities;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Persistence.Seeders;

/// <summary>
/// Idempotentno popunjava bazu početnim podacima: admin nalog, kategorije i originalnih 7 postova.
/// Bezbedno se poziva na svakom startu — preskače ono što već postoji.
/// </summary>
public static partial class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IPasswordHasher passwordHasher, CancellationToken ct = default)
    {
        var admin = await EnsureAdminAsync(db, passwordHasher, ct);
        var categories = await EnsureCategoriesAsync(db, ct);
        await EnsurePostsAsync(db, admin, categories, ct);
    }

    private static async Task<User> EnsureAdminAsync(AppDbContext db, IPasswordHasher hasher, CancellationToken ct)
    {
        var admin = await db.Users.FirstOrDefaultAsync(u => u.Username == "luka", ct);
        if (admin is not null)
        {
            return admin;
        }

        admin = new User
        {
            Username = "luka",
            Email = "luka.vukovic288@gmail.com",
            DisplayName = "Luka Vukovic",
            Bio = "Cybersecurity enthusiast and developer. Blogging about my IT journey.",
            Role = UserRole.Admin,
            PasswordHash = hasher.Hash("Admin123!"),
        };

        db.Users.Add(admin);
        await db.SaveChangesAsync(ct);
        return admin;
    }

    private static async Task<Dictionary<string, Category>> EnsureCategoriesAsync(AppDbContext db, CancellationToken ct)
    {
        var map = new Dictionary<string, Category>();

        foreach (var seed in BlogSeedData.Categories)
        {
            var category = await db.Categories.FirstOrDefaultAsync(c => c.Slug == seed.Slug, ct);
            if (category is null)
            {
                category = new Category { Name = seed.Name, Slug = seed.Slug, Description = seed.Description };
                db.Categories.Add(category);
            }

            map[seed.Slug] = category;
        }

        await db.SaveChangesAsync(ct);
        return map;
    }

    private static async Task EnsurePostsAsync(
        AppDbContext db, User admin, Dictionary<string, Category> categories, CancellationToken ct)
    {
        var tagCache = await db.Tags.ToDictionaryAsync(t => t.Slug, ct);
        var publishedBase = new DateTime(2024, 7, 1, 12, 0, 0, DateTimeKind.Utc);
        var index = 0;

        foreach (var seed in BlogSeedData.Posts)
        {
            index++;

            if (await db.Posts.AnyAsync(p => p.Slug == seed.Slug, ct))
            {
                continue;
            }

            var post = new Post
            {
                Slug = seed.Slug,
                Title = seed.Title,
                Excerpt = seed.Excerpt,
                ContentHtml = seed.ContentHtml,
                CoverImageUrl = seed.CoverImageUrl,
                Status = PostStatus.Published,
                PublishedAt = publishedBase.AddDays(index),
                ReadMinutes = EstimateReadMinutes(seed.ContentHtml),
                AuthorId = admin.Id,
                CategoryId = categories.TryGetValue(seed.CategorySlug, out var category) ? category.Id : null,
            };

            foreach (var tagName in seed.Tags)
            {
                var slug = Slug.Generate(tagName);
                if (!tagCache.TryGetValue(slug, out var tag))
                {
                    tag = new Tag { Name = tagName, Slug = slug };
                    db.Tags.Add(tag);
                    tagCache[slug] = tag;
                }

                post.PostTags.Add(new PostTag { Tag = tag });
            }

            db.Posts.Add(post);
        }

        await db.SaveChangesAsync(ct);
    }

    private static int EstimateReadMinutes(string html)
    {
        var text = HtmlTags().Replace(html, " ");
        var words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Max(1, (int)Math.Ceiling(words / 200.0));
    }

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex HtmlTags();
}
