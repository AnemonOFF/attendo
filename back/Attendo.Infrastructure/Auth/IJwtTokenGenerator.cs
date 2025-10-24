using Attendo.Domain.Entities;

namespace Attendo.Infrastructure.Auth;

public interface IJwtTokenGenerator
{
    (string AccessToken, DateTime ExpiresAt) CreateToken(User user);
}
