using Attendo.Domain.Entities;

namespace Attendo.Infrastructure.Users;

public interface IUserRepository
{
    Task<User?> FindByLoginOrEmailAsync(string loginOrEmail, CancellationToken ct);
    Task<User?> FindByIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsLoginAsync(string login, CancellationToken ct);
    Task<bool> ExistsEmailAsync(string email, CancellationToken ct);
    Task<User> AddAsync(User user, CancellationToken ct);
}
