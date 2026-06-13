using AutoMapper;
using Blog.Application.Auth.Dtos;
using Blog.Application.Common.Interfaces;
using Blog.Application.Common.Settings;
using Blog.Application.Common.Utilities;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Blog.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository users;
    private readonly IRefreshTokenRepository refreshTokens;
    private readonly IPasswordHasher passwordHasher;
    private readonly IJwtService jwt;
    private readonly IMapper mapper;
    private readonly JwtSettings settings;
    private readonly IValidator<RegisterRequest> registerValidator;
    private readonly IValidator<LoginRequest> loginValidator;

    public AuthService(
        IUserRepository users,
        IRefreshTokenRepository refreshTokens,
        IPasswordHasher passwordHasher,
        IJwtService jwt,
        IMapper mapper,
        IOptions<JwtSettings> settings,
        IValidator<RegisterRequest> registerValidator,
        IValidator<LoginRequest> loginValidator)
    {
        this.users = users;
        this.refreshTokens = refreshTokens;
        this.passwordHasher = passwordHasher;
        this.jwt = jwt;
        this.mapper = mapper;
        this.settings = settings.Value;
        this.registerValidator = registerValidator;
        this.loginValidator = loginValidator;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        await registerValidator.ValidateAndThrowAsync(request, ct);

        var email = request.Email.Trim();
        var username = request.Username.Trim();

        if (await users.ExistsAsync(email, username, ct))
        {
            throw new InvalidOperationException("Korisnik sa tim email-om ili korisničkim imenom već postoji.");
        }

        var user = new User
        {
            Username = username,
            Email = email,
            DisplayName = string.IsNullOrWhiteSpace(request.DisplayName) ? username : request.DisplayName.Trim(),
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = UserRole.Reader,
        };

        await users.AddAsync(user, ct);
        await users.SaveChangesAsync(ct);

        return await IssueTokensAsync(user, ct);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        await loginValidator.ValidateAndThrowAsync(request, ct);

        var user = await users.GetByUsernameOrEmailAsync(request.UsernameOrEmail.Trim(), ct);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Pogrešno korisničko ime/email ili lozinka.");
        }

        return await IssueTokensAsync(user, ct);
    }

    public async Task<AuthResponse> RefreshAsync(string refreshToken, CancellationToken ct = default)
    {
        var stored = await refreshTokens.GetByTokenAsync(RefreshTokenHasher.Hash(refreshToken), ct);

        if (stored is null || !stored.IsActive)
        {
            throw new UnauthorizedAccessException("Refresh token nije validan ili je istekao.");
        }

        // Rotacija: stari token se opoziva, izdaje se novi par.
        stored.RevokedAt = DateTime.UtcNow;
        await refreshTokens.SaveChangesAsync(ct);

        return await IssueTokensAsync(stored.User, ct);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken ct = default)
    {
        var stored = await refreshTokens.GetByTokenAsync(RefreshTokenHasher.Hash(refreshToken), ct);

        if (stored is { RevokedAt: null })
        {
            stored.RevokedAt = DateTime.UtcNow;
            await refreshTokens.SaveChangesAsync(ct);
        }
    }

    public async Task<UserDto> GetByIdAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await users.GetByIdAsync(userId, ct)
            ?? throw new KeyNotFoundException("Korisnik nije pronađen.");

        return mapper.Map<UserDto>(user);
    }

    private async Task<AuthResponse> IssueTokensAsync(User user, CancellationToken ct)
    {
        var accessToken = jwt.CreateAccessToken(user);
        var refreshToken = jwt.CreateRefreshToken();

        await refreshTokens.AddAsync(
            new RefreshToken
            {
                UserId = user.Id,
                Token = RefreshTokenHasher.Hash(refreshToken),
                ExpiresAt = DateTime.UtcNow.AddDays(settings.RefreshTokenDays),
            },
            ct);
        await refreshTokens.SaveChangesAsync(ct);

        return new AuthResponse(
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(settings.AccessTokenMinutes),
            mapper.Map<UserDto>(user));
    }
}
