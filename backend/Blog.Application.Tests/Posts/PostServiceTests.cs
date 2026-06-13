using AutoMapper;
using Blog.Application.Common.Exceptions;
using Blog.Application.Posts;
using Blog.Application.Posts.Dtos;
using Blog.Application.Posts.Mapping;
using Blog.Application.Posts.Validators;
using Blog.Application.Tags.Mapping;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Blog.Application.Tests.Posts;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> posts = new();
    private readonly Mock<ITagRepository> tags = new();
    private readonly IMapper mapper;
    private readonly PostService sut;

    public PostServiceTests()
    {
        var mapperConfig = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<PostProfile>();
                cfg.AddProfile<TagProfile>();
            },
            NullLoggerFactory.Instance);
        mapper = mapperConfig.CreateMapper();

        tags
            .Setup(t => t.GetBySlugsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        sut = new PostService(
            posts.Object,
            tags.Object,
            mapper,
            new CreatePostRequestValidator(),
            new UpdatePostRequestValidator());
    }

    [Fact]
    public async Task CreateAsync_GeneratesUniqueSlug_AndPublishesWhenRequested()
    {
        var request = new CreatePostRequest(
            Title: "Moj prvi post",
            Excerpt: "Kratak opis",
            ContentHtml: "<p>Sadržaj posta</p>",
            CoverImageUrl: null,
            CategoryId: null,
            TagNames: [],
            Publish: true);

        var authorId = Guid.NewGuid();

        posts
            .Setup(p => p.SlugExistsAsync("moj-prvi-post", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Post? added = null;
        posts
            .Setup(p => p.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Callback<Post, CancellationToken>((post, _) => added = post)
            .Returns(Task.CompletedTask);

        posts
            .Setup(p => p.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => added);

        var result = await sut.CreateAsync(request, authorId, CancellationToken.None);

        Assert.Equal("moj-prvi-post", result.Slug);
        Assert.Equal("Published", result.Status);
        Assert.NotNull(added);
        Assert.Equal(authorId, added!.AuthorId);
        Assert.NotNull(added.PublishedAt);
        posts.Verify(p => p.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithoutPublish_SavesAsDraft()
    {
        var request = new CreatePostRequest(
            Title: "Draft post",
            Excerpt: "Opis",
            ContentHtml: "<p>Sadržaj</p>",
            CoverImageUrl: null,
            CategoryId: null,
            TagNames: [],
            Publish: false);

        posts
            .Setup(p => p.SlugExistsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Post? added = null;
        posts
            .Setup(p => p.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
            .Callback<Post, CancellationToken>((post, _) => added = post)
            .Returns(Task.CompletedTask);

        posts
            .Setup(p => p.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => added);

        var result = await sut.CreateAsync(request, Guid.NewGuid(), CancellationToken.None);

        Assert.Equal("Draft", result.Status);
        Assert.Null(added!.PublishedAt);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyTitle_ThrowsValidationException()
    {
        var request = new CreatePostRequest(
            Title: "",
            Excerpt: "Opis",
            ContentHtml: "<p>Sadržaj</p>",
            CoverImageUrl: null,
            CategoryId: null,
            TagNames: [],
            Publish: false);

        await Assert.ThrowsAsync<ValidationException>(
            () => sut.CreateAsync(request, Guid.NewGuid(), CancellationToken.None));

        posts.Verify(p => p.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetBySlugAsync_WhenPostExists_IncrementsViewCount()
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Slug = "post-slug",
            Title = "Naslov",
            Excerpt = "Opis",
            ContentHtml = "<p>Sadržaj</p>",
            Status = PostStatus.Published,
            ViewCount = 5,
            Author = new User { DisplayName = "Autor" },
        };

        posts
            .Setup(p => p.GetBySlugAsync("post-slug", false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var result = await sut.GetBySlugAsync("post-slug", CancellationToken.None);

        Assert.Equal(6, post.ViewCount);
        Assert.Equal("Naslov", result.Title);
        posts.Verify(p => p.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBySlugAsync_WhenPostMissing_ThrowsKeyNotFound()
    {
        posts
            .Setup(p => p.GetBySlugAsync("missing", false, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Post?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => sut.GetBySlugAsync("missing", CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_RemovesPost_AndSaves()
    {
        var authorId = Guid.NewGuid();
        var post = new Post { Id = Guid.NewGuid(), AuthorId = authorId, Author = new User() };

        posts
            .Setup(p => p.GetByIdAsync(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        await sut.DeleteAsync(post.Id, authorId, isAdmin: false, CancellationToken.None);

        posts.Verify(p => p.Remove(post), Times.Once);
        posts.Verify(p => p.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotOwnerAndNotAdmin_ThrowsUnauthorized()
    {
        var post = new Post { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid(), Author = new User() };

        posts
            .Setup(p => p.GetByIdAsync(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        await Assert.ThrowsAsync<ForbiddenAccessException>(
            () => sut.DeleteAsync(post.Id, Guid.NewGuid(), isAdmin: false, CancellationToken.None));

        posts.Verify(p => p.Remove(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenAdmin_AllowsDeletingOthersPost()
    {
        var post = new Post { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid(), Author = new User() };

        posts
            .Setup(p => p.GetByIdAsync(post.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        await sut.DeleteAsync(post.Id, Guid.NewGuid(), isAdmin: true, CancellationToken.None);

        posts.Verify(p => p.Remove(post), Times.Once);
    }
}
