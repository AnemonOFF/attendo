using Attendo.Domain.Entities;
using Attendo.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Infrastructure.Users;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> FindByLoginOrEmailAsync(string loginOrEmail, CancellationToken ct) =>
        _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == loginOrEmail || u.Email == loginOrEmail, ct);

    public Task<bool> ExistsLoginAsync(string login, CancellationToken ct) =>
        _db.Users.AnyAsync(u => u.Login == login, ct);

    public Task<bool> ExistsEmailAsync(string email, CancellationToken ct) =>
        _db.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<User> AddAsync(User user, CancellationToken ct)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return user;
    }
}
