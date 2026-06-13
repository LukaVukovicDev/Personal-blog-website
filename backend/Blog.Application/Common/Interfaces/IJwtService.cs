using Blog.Domain.Entities;

namespace Blog.Application.Common.Interfaces;

public interface IJwtService
{
    string CreateAccessToken(User user);

    string CreateRefreshToken();
}
